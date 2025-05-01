using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using SspUis.RabbitMQ.Publishers;
using SspUis.RabbitMQ.Messages;
using WEBASE;

namespace SspUis.RabbitMQ.Messages
{
    public record ErrorQueueMessage : QueueMessage
    {
        public string Environment { get; set; }
        public int UserId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Host { get; set; } = null!;
        public string RequestPath { get; set; } = null!;
        public string? RequestParams { get; set; }
        public object RequestBody { get; set; }
        public ProblemDetails Details { get; set; } = null!;
    }
}
