using SspUis.RabbitMQ;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.RabbitMQ.Application.Publishers;
using SspUis.RabbitMQ.Certificate.Messages;
using SspUis.RabbitMQ.Certificate.Publishers;
using SspUis.RabbitMQ.CustomJob.Messages;
using SspUis.RabbitMQ.CustomJob.Publishers;

namespace Microsoft.Extensions.DependencyInjection;

public static class RabbitServiceExtentions
{
    public static void ConfigureRabbitServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddRabbitMQ(options =>
        {
            config.GetSection("RabbitMQ").Bind(options);
        })
        .AddPublisher<PrtnApplicationMessage, PrtnApplicationPublisher>()
        .AddPublisher<PrtnCertificateMessage, PrtnCertificatePublisher>()
        .AddPublisher<MonoApplicationMessage, MonoApplicationPublisher>()
        .AddPublisher<CustomJobMessage, CustomJobPublisher>()
        ;
    }
}
