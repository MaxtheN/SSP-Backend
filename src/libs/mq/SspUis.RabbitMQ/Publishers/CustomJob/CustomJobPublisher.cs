using Microsoft.Extensions.Logging;
using SspUis.Core.Security;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.RabbitMQ.CustomJob.Messages;

namespace SspUis.RabbitMQ.CustomJob.Publishers
{
    public class CustomJobPublisher : PublisherBase<CustomJobMessage>
    {
        public CustomJobPublisher(IAuthService authService, IRabbitMQClient client, ILogger<CustomJobPublisher> logger)
            : base(authService, client, logger, Queues.Job.CustomJob)
        {
        }
    }
}
