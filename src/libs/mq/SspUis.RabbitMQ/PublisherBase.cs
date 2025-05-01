using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SspUis.RabbitMQ.Extensions;
using SspUis.RabbitMQ.Models;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using SspUis.RabbitMQ.Abstractions;
using StatusGeneric;
using SspUis.RabbitMQ.Messages;
using Newtonsoft.Json;
using SspUis.Core.Security;

namespace SspUis.RabbitMQ
{
    public abstract class PublisherBase<TMessage> : StatusGenericHandler, IPublisher<TMessage>
        where TMessage : QueueMessage
    {
        private readonly ILogger<PublisherBase<TMessage>> _logger;
        private readonly IAuthService _authService;
        private readonly IRabbitMQClient _client;
        private readonly int _retryCount;
        private readonly RabbitQueue _queue;

        public PublisherBase(IRabbitMQClient client, ILogger<PublisherBase<TMessage>> logger, RabbitQueue queue, int retryCount = 5)
            : this(null, client, logger, queue, retryCount)
        {
        }

        public PublisherBase(IAuthService authService, IRabbitMQClient client, ILogger<PublisherBase<TMessage>> logger, RabbitQueue queue, int retryCount = 5)
        {
            _authService = authService;
            _client = client;
            _retryCount = retryCount;
            _queue = queue;
            _logger = logger;

            //Declare();
        }

        public virtual void Publish(TMessage message)
        {
            try
            {
                // to ensure the queue was declared
                //Declare();

                if (!_client.IsConnected)
                    _client.TryConnect();

                var policy = Policy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "Could not publish message: {EventId} after {Timeout}s ({ExceptionMessage})", message.Id, $"{time.TotalSeconds:n1}", ex.Message);
                    });

                //using (var channel = _client.CreateModel(_queue.Name))
                //{
                //    policy.Execute(() =>
                //    {
                //        _logger.LogInformation($"Publishing message to [{_queue.Name}]: {{ message-id: {message.Id}, pulisher: {_client.AppName} }}");
                //        channel.Publish(_queue.Exchange, _queue.RoutingKey, message);
                //        _logger.LogInformation($"Published message to [{_queue.Name}]: {{ message-id: {message.Id}, pulisher: {_client.AppName} }}");
                //        _logger.LogDebug($"{message.Id}:\n{JsonConvert.SerializeObject(message, Formatting.Indented)}");
                //    });
                //}

                var channel = _client.CreateModel(_queue.Name);
                policy.Execute(() =>
                {
                    _logger.LogInformation($"Publishing message to [{_queue.Name}]: {{ message-id: {message.Id}, pulisher: {_client.AppName} }}");
                    channel.Publish(_queue.Exchange, _queue.RoutingKey, message);
                    _logger.LogInformation($"Published message to [{_queue.Name}]: {{ message-id: {message.Id}, pulisher: {_client.AppName} }}");
                    //_logger.LogDebug($"{message.Id}:\n{JsonConvert.SerializeObject(message, Formatting.Indented)}");
                });
            }
            catch (Exception ex)
            {
                AddError($"Could not publish message. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Declare exchanges, queues and bindings
        /// </summary>
        public void Declare()
        {
            using (var channel = _client.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: _queue.Exchange,
                    type: _queue.ExchangeType,
                    durable: true,
                    autoDelete: false);
                channel.QueueDeclare(
                    queue: _queue.Name,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);
                channel.QueueBind(
                    queue: _queue.Name,
                    exchange: _queue.Exchange,
                    routingKey: _queue.RoutingKey);
            }

            _logger.LogInformation($"Declared queue '{_queue.Name}'.");
        }
    }
}
