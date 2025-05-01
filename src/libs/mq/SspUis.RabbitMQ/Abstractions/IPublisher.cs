using StatusGeneric;
using SspUis.RabbitMQ.Messages;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ.Abstractions
{
    public interface IPublisher<in TEvent> : IStatusGeneric
        where TEvent : QueueMessage
    {
        void Declare();
        void Publish(TEvent message);
    }

}
