using Microsoft.Extensions.Logging;
using SspUis.Core.Security;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Application.Messages;

namespace SspUis.RabbitMQ.Application.Publishers
{
    public class StateAssetApplicationPublisher : PublisherBase<StateAssetApplicationMessage>
    {
        public StateAssetApplicationPublisher(IAuthService authService, IRabbitMQClient client, ILogger<StateAssetApplicationPublisher> logger)
            : base(authService, client, logger, Queues.StateAsset.StateAssetApplication)
        {
        }
    }
}
