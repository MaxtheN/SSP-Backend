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
    public abstract class PublisherMultipleBase<TMessage> : StatusGenericHandler, IPublisher<TMessage>
        where TMessage : QueueMessage
    {
        private readonly ILogger<PublisherMultipleBase<TMessage>> _logger;
        private readonly IAuthService _authService;
        private readonly IRabbitMQClient _client;
        private readonly int _retryCount;
        private readonly List<RabbitQueue> _queues;
        public List<RabbitQueue> QueueList
        {
            get
            {
                return _queues;
            }
        }

        public PublisherMultipleBase(IRabbitMQClient client, ILogger<PublisherMultipleBase<TMessage>> logger, List<RabbitQueue> queues, int retryCount = 5)
            : this(null, client, logger, queues, retryCount)
        {
        }

        public PublisherMultipleBase(IAuthService authService, IRabbitMQClient client, ILogger<PublisherMultipleBase<TMessage>> logger, List<RabbitQueue> queues, int retryCount = 5)
        {
            _authService = authService;
            _client = client;
            _retryCount = retryCount;
            _queues = queues;
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

                foreach (var queue in _queues)
                {
                    var channel = _client.CreateModel(queue.Name);
                    policy.Execute(() =>
                    {
                        _logger.LogInformation($"Publishing message to [{queue.Name}]: {{ message-id: {message.Id}, pulisher: {_client.AppName} }}");
                        channel.Publish(queue.Exchange, queue.RoutingKey, message);
                        _logger.LogInformation($"Published message to [{queue.Name}]: {{ message-id: {message.Id}, pulisher: {_client.AppName} }}");
                        //_logger.LogDebug($"{message.Id}:\n{JsonConvert.SerializeObject(message, Formatting.Indented)}");
                    });
                }
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
                foreach (var queue in _queues)
                {
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

                    _logger.LogInformation($"Declared queue '{queue.Name}'.");
                }
            }

        }
    }
}
