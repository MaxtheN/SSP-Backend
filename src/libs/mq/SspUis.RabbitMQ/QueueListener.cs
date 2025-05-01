using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SspUis.Core;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Extensions;
using SspUis.RabbitMQ.Messages;
using SspUis.RabbitMQ.Publishers;
using WEBASE;
using WEBASE.AspNet;
using System.Data.Common;

namespace SspUis.RabbitMQ
{
    public class QueueListener<TMessage> : BackgroundService
        where TMessage : QueueMessage
    {
        private readonly ILogger<QueueListener<TMessage>> _logger;
        private readonly IRabbitMQClient _client;
        private readonly IConsumerConfig<TMessage> _consumerConfig;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeAccessor _serviceScopeAccessor;
        private readonly DbContext _dbContext;

        public QueueListener(
            IRabbitMQClient client,
            IConsumerConfig<TMessage> consumerConfig,
            IServiceProvider serviceProvider,
            IServiceScopeAccessor serviceScopeAccessor,
            ILogger<QueueListener<TMessage>> logger)
        {
            _client = client;
            _consumerConfig = consumerConfig;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _serviceScopeAccessor = serviceScopeAccessor;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ Listener for {QueueName} started", _consumerConfig.Queue.Name);

            Declare();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("RabbitMQ Listener for {QueueName} stopped", _consumerConfig.Queue);
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (int workerIndex = 1; workerIndex <= _consumerConfig.WorkerCount; workerIndex++)
            {
                RunWorker($"[{_consumerConfig.Name} Worker {workerIndex}]", cancellationToken);
            }

            return Task.CompletedTask;
        }

        private void RunWorker(string workerName, CancellationToken cancellationToken)
        {
            var channel = _client.CreateModel();

            // NOTE:
            // global: false => shared across all consumers on the channel (applied separately to each new consumer on the channel)
            // global: true => shared across all consumers on the connection (shared across all consumers on the channel)
            channel.BasicQos(0, /*_consumer.PrefetchCount*/ 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (_, e) =>
            {
                TMessage message = null;
                long jobHistoryId = 0;
                try
                {
                    message = e.To<TMessage>();
                    jobHistoryId = message.JobHistoryId;

                    _logger.LogInformation($"Message received: (queue: {_consumerConfig.Queue.Name}, worker: {workerName})");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        _serviceScopeAccessor.Scope = scope;
                        try
                        {
                            var consumerInstance = scope.ServiceProvider.GetRequiredService<IConsumer<TMessage>>();
                            await consumerInstance.ConsumeAsync(message, cancellationToken);
                            if (consumerInstance.HasErrors)
                                throw new Exception(consumerInstance.GetAllErrors());
                        }
                        finally
                        {
                            _serviceScopeAccessor.Scope = null;
                        }
                    }
                    channel.BasicAck(e.DeliveryTag, false);

                    _logger.LogInformation($"Message processed: (queue: {_consumerConfig.Queue.Name}, worker: {workerName})");
                }
                catch (Exception ex)
                {
                    try
                    {
                        channel.BasicNack(e.DeliveryTag, false, _consumerConfig.RequeueOnFailed);
                        _logger.LogError(ex, $"Unexpected error while processing message: (queue: {_consumerConfig.Queue.Name}, worker: {workerName})");

                        if (jobHistoryId > 0)
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

                                using (var command = dbContext.Database.GetDbConnection().CreateCommand())
                                {
                                    if (command.Connection?.State != System.Data.ConnectionState.Open)
                                        command.Connection?.Open();

                                    command.CommandText = $"SELECT STATUS_ID FROM SYS_JOB_HISTORY WHERE ID={jobHistoryId}";
                                    var statusId = command.ExecuteScalar().AsString();

                                    if (statusId.AsInt() == StatusIdConst.WAITING)
                                    {
                                        var exceptionMessage = $"{ex.Message}. {ex.InnerException}";
                                        var errorMessage = exceptionMessage.Length > 500 ? exceptionMessage.Substring(0, 499) : exceptionMessage;
                                        command.CommandText = $"UPDATE SYS_JOB_HISTORY SET STATUS_ID = {StatusIdConst.FAILED}, MESSAGE='{errorMessage}' WHERE ID={jobHistoryId}";
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // push error log
                        PublishError(ex, message);

                    }
                    catch (Exception ex1)
                    {
                        // don't throw here
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();
            };

            channel.BasicConsume(_consumerConfig.Queue.Name, false, consumer);
        }

        /// <summary>
        /// Declare exchanges, queues and bindings
        /// </summary>
        private void Declare()
        {
            using var channel = _client.CreateModel();

            var queue = _consumerConfig.Queue;

            channel.ExchangeDeclare(
                exchange: queue.Exchange,
                type: queue.ExchangeType,
                durable: true,
                autoDelete: false);
            channel.QueueDeclare(
                queue: queue.Name,
                durable: true,
                exclusive: false,
                autoDelete: false);
            channel.QueueBind(
                queue: queue.Name,
                exchange: queue.Exchange,
                routingKey: queue.RoutingKey);
        }

        private async void PublishError(Exception exception, TMessage message)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var errorPublisher = scope.ServiceProvider.GetRequiredService<ErrorPublisher>();

                    var problem = new ProblemDetails
                    {
                        Status = 500,
                        Type = exception.GetType().Name,
                        Title = exception.Message,
                        Detail = $"StackTrace: {Environment.NewLine}{exception.StackTrace}{Environment.NewLine} Full exception: {Environment.NewLine}{exception}{Environment.NewLine}Inner: {exception.InnerException}",
                    };
                    var error = new ErrorMessage
                    {
                        Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                        ServiceName = AppDomain.CurrentDomain.FriendlyName,
                        CurrentOrganizationId = message?.OrganizationId ?? 0,
                        UserId = 0,
                        UserName = message?.UserName,
                        //Host = context.Request.Host.Value,
                        //RequestPath = context.Request.Path,
                        //RequestBody = await ReadRequestBodyAsync(context),
                        Details = problem,
                        RequestTraceId = message?.RequestTraceId
                    };

                    //errorPublisher.Publish(error);
                    //await TelegramBotHelper.SendJobErrorToChannelAsFileAsync(error, _consumerConfig.Queue.Name, message);
                }
            }
            //catch (ApiRequestException ex)
            //{
            //    _logger.LogCritical(ex, $"An error occurred while sending 'CONSUMER_ERROR_DETAILS' to telegram channnel.");
            //}
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while publishing error in QUEUE_LISTENER.");
            }
        }
    }
}
