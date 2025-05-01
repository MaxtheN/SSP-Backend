using Microsoft.Extensions.DependencyInjection;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Messages;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitMQConfig> configAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configAction == null) throw new ArgumentNullException(nameof(configAction));

            var configurationInstance = new RabbitMQConfig();
            configAction(configurationInstance);

            services.AddSingleton(configurationInstance);

            // register rabbit client
            services.AddSingleton<IRabbitMQClient, RabbitMQClient>();

            return services;
        }

        public static IServiceCollection AddPublisher<TMessage, TPublisher>(this IServiceCollection services)
           where TMessage : QueueMessage
           where TPublisher : class, IPublisher<TMessage>
        {
            var serviceProvider = services.AddScoped<TPublisher>().BuildServiceProvider();

            var publisher = serviceProvider.GetRequiredService<TPublisher>();
            if (publisher != null)
                publisher.Declare();

            return services;
        }

        public static IServiceCollection AddConsumers<TMessage, TConsumerConfig, TConsumer>(this IServiceCollection services, Action<TConsumerConfig> configAction)
            where TMessage : QueueMessage
            where TConsumer : class, IConsumer<TMessage>
            where TConsumerConfig : class, IConsumerConfig<TMessage>
        {
            if (configAction == null) throw new ArgumentNullException(nameof(configAction));

            services.AddHostedService<QueueListener<TMessage>>();
            services.AddScoped<IConsumer<TMessage>, TConsumer>();

            services.AddSingleton<IConsumerConfig<TMessage>, TConsumerConfig>(sp =>
            {
                var configurationInstance = Activator.CreateInstance<TConsumerConfig>();
                configAction(configurationInstance);

                return configurationInstance;
            });

            return services;
        }
    }
}
