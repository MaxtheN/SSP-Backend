using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Collections.Concurrent;
using System.Net.Sockets;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ
{
    public class RabbitMQClient : IRabbitMQClient, IDisposable
    {
        private readonly RabbitMQConfig _config;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQClient> _logger;
        private readonly int _retryCount;

        private IConnection _connection;
        private bool _disposed;

        private readonly object _syncRoot = new();

        protected readonly ConcurrentDictionary<string, IModel> Channels = new ConcurrentDictionary<string, IModel>();

        public RabbitMQClient(RabbitMQConfig config, ILogger<RabbitMQClient> logger, int retryCount = 5)
        {
            _config = config;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;

            _connectionFactory = new ConnectionFactory
            {
                DispatchConsumersAsync = true,
                HostName = _config.Host,
                Port = _config.Port,
                VirtualHost = _config.VirtualHost,
                UserName = _config.UserName,
                Password = _config.Password,
                AutomaticRecoveryEnabled = true,
            };

            TryConnect();
        }

        public string AppName => _config.AppName;
        public bool IsConnected => _connection is { IsOpen: true } && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public IModel CreateModel(string queueName)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            if (Channels.ContainsKey(queueName))
                return Channels[queueName];

            var channel = _connection.CreateModel();

            Channels[queueName] = channel;
            return channel;
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (_syncRoot)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}'", _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is blocked. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _logger.LogInformation("Closing RabbitMQ connections...");

                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}