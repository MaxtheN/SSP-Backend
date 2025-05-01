namespace SspUis.RabbitMQ.Models
{
    public class RabbitQueue
    {
        /// <summary>
        /// Queue name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sender name
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// Receiver name
        /// </summary>
        public string Consumer { get; set; }
        public string Exchange { get; set; }
        public string ExchangeType { get; set; }
        public string RoutingKey { get; set; }
    }
}
