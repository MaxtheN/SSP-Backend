using Microsoft.Extensions.Logging;
using SspUis.Core.Security;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Application.Messages;

namespace SspUis.RabbitMQ.Application.Publishers
{
    public class PrtnApplicationPublisher : PublisherBase<PrtnApplicationMessage>
    {
        public PrtnApplicationPublisher(IAuthService authService, IRabbitMQClient client, ILogger<PrtnApplicationPublisher> logger)
            : base(authService, client, logger, Queues.Partner.PrtnApplication)    
        {
        }
    }
}
