namespace SspUis.RabbitMQ.Messages
{
    public record DocumentMessage : QueueMessage
    {
        public long DocId { get; set; }
        public int TableId { get; set; }
        public int? FromStatusId { get; set; }
        public int ToStatusId { get; set; }
    }
}
