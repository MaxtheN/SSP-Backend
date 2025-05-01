using Microsoft.Extensions.Logging;
using StatusGeneric;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Messages;

namespace SspUis.RabbitMQ.Doc
{
    public abstract class ConsumerBase<TMessage> : StatusGenericHandler, IConsumer<TMessage>
        where TMessage : QueueMessage
    {
        private readonly ILogger _logger;

        public ConsumerBase(ILogger<ConsumerBase<TMessage>> logger)
        {
            _logger = logger;
        }

        public abstract Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);
    }
}
