using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.RabbitMQ.Messages
{
    public record QueueMessage
    {
        public QueueMessage()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public QueueMessage(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        public DateTime CreationDate { get; private init; }
        public string UserName { get; set; }
        public int? OrganizationId { get; set; }
        public long JobHistoryId { get; set; }
        public string? RequestTraceId { get; set; }
        public string UserIp { get; set; }
        public string UserAgent { get; set; }
    }
}
