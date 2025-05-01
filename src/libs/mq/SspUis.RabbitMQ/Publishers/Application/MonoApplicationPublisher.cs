using Microsoft.Extensions.Logging;
using SspUis.Core.Security;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Application.Messages;

namespace SspUis.RabbitMQ.Application.Publishers
{
    public class MonoApplicationPublisher : PublisherBase<MonoApplicationMessage>
    {
        public MonoApplicationPublisher(IAuthService authService, IRabbitMQClient client, ILogger<MonoApplicationPublisher> logger)
            : base(authService, client, logger, Queues.Mono.MonoApplication)    
        {
        }
    }
}
