using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Channels;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ.Extensions
{
    /// <summary>
    /// RabbitMQ extension methods
    /// </summary> 
    public static class RabbitMqExt
    {
        /// <summary>
        /// Publish a message as json
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        /// <param name="msg"></param>
        public static void Publish<TMessage>(this IModel channel, string exchange, string routingKey, TMessage msg)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // Does persist to disk

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));

            channel.BasicPublish(exchange, routingKey, properties, data);
        }

        /// <summary>
        /// Maps message body to a model
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static TMessage To<TMessage>(this BasicDeliverEventArgs e)
        {
            var json = Encoding.UTF8.GetString(e.Body.Span);
            return JsonConvert.DeserializeObject<TMessage>(json);
        }
    }
}
