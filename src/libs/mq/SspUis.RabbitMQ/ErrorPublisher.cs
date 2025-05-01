using Microsoft.Extensions.Logging;
using SspUis.Core.Security;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Messages;

namespace SspUis.RabbitMQ.Publishers
{
    public class ErrorPublisher : PublisherBase<ErrorQueueMessage>
    {
        public ErrorPublisher(IAuthService authService, IRabbitMQClient client, ILogger<ErrorPublisher> logger)
            : base(authService, client, logger, Queues.Error)
        {
        }
    }
}
