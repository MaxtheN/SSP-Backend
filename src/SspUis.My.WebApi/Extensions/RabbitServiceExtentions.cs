using SspUis.RabbitMQ;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.RabbitMQ.Application.Consumers;
using SspUis.DataLayer.EfClasses;
using SspUis.RabbitMQ.Application.Publishers;
using SspUis.RabbitMQ.CustomJob.Publishers;
using SspUis.RabbitMQ.CustomJob.Messages;
/*

using Uzasbo2.RabbitMQ.Doc.Asset.Consumers;
using Uzasbo2.RabbitMQ.Doc.Asset.Messages;*/

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
            .AddPublisher<PrtnApplicationMessage, PrtnApplicationPublisher>()
            .AddPublisher<MonoApplicationMessage, MonoApplicationPublisher>()
            .AddPublisher<StateAssetApplicationMessage, StateAssetApplicationPublisher>()
            .AddPublisher<CustomJobMessage, CustomJobPublisher>()
            //.AddConsumers<PrtnApplicationMessage, PrtnApplicationConsumerConfig, PrtnApplicationConsumer>(options =>
            //{
            //    config.GetSection("RabbitMQ:Consumers:PrtnApplication").Bind(options);
            //})
            ;
        }
    }
}
