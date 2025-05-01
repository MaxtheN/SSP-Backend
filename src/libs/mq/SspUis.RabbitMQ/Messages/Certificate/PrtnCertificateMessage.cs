using SspUis.RabbitMQ.Messages;

namespace SspUis.RabbitMQ.Certificate.Messages
{
    public record PrtnCertificateMessageBase : DocumentMessage
    {
    }

    public record PrtnCertificateMBMessage : PrtnCertificateMessageBase
    {
    }
    public record PrtnCertificateBojxonaMessage : PrtnCertificateMessageBase
    {
    }
    public record PrtnCertificateMessage : PrtnCertificateMessageBase
    {
    }
    public record PrtnCertificateMoliyaMessage : PrtnCertificateMessageBase
    {

    }
}
