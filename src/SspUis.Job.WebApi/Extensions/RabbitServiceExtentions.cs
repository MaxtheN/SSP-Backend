using SspUis.RabbitMQ;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.RabbitMQ.Application.Consumers;
using SspUis.DataLayer.EfClasses;
using SspUis.RabbitMQ.Application.Publishers;
using SspUis.RabbitMQ.Certificate.Messages;
using SspUis.RabbitMQ.Certificate.Consumers;
using SspUis.RabbitMQ.CustomJob.Messages;
using SspUis.RabbitMQ.customJob.Consumers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitServiceExtentions
    {
        public static void ConfigureRabbitServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddRabbitMQ(options =>
            {
                config.GetSection("RabbitMQ").Bind(options);
            })
            .AddConsumers<PrtnApplicationMessage, PrtnApplicationConsumerConfig, PrtnApplicationConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:PrtnApplication").Bind(options);
            })
            .AddConsumers<MonoApplicationMessage, MonoApplicationConsumerConfig, MonoApplicationConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:MonoApplication").Bind(options);
            })
            .AddConsumers<PrtnCertificateMessage, PrtnCertificateConsumerConfig, PrtnCertificateConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:PrtnCertificate").Bind(options);
            })
            .AddConsumers<PrtnCertificateBojxonaMessage, PrtnCertificateBojxonaConsumerConfig, PrtnCertificateBojxonaConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:PrtnCertificateBojxona").Bind(options);
            })
            .AddConsumers<PrtnCertificateMBMessage, PrtnCertificateMBConsumerConfig, PrtnCertificateMBConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:PrtnCertificateMB").Bind(options);
            })
            .AddConsumers<CustomJobMessage, CustomJobConsumerConfig, CustomJobConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:CustomJob").Bind(options);
            })
            .AddConsumers<StateAssetApplicationMessage, StateAssetApplicationConsumerConfig, StateAssetApplicationConsumer>(options =>
            {
                config.GetSection("RabbitMQ:Consumers:StateAssetApplication").Bind(options);
            })
            ;
        }
    }
}
