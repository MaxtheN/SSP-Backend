using Microsoft.Extensions.Logging;
using SspUis.Core.Security;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Certificate.Messages;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ.Certificate.Publishers
{
    public class PrtnCertificatePublisher : PublisherMultipleBase<PrtnCertificateMessageBase>
    {
        public PrtnCertificatePublisher(IAuthService authService, IRabbitMQClient client, ILogger<PrtnCertificatePublisher> logger)
            : base(authService, client, logger, new List<RabbitQueue>() { Queues.Partner.PrtnCertificate, Queues.Partner.PrtnCertificateBojxona, Queues.Partner.PrtnCertificateMB, Queues.Partner.PrtnCertificateMoliya })
        {

        }
    }
}
