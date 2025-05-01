using Microsoft.AspNetCore.Mvc;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ.Abstractions
{
    public interface IConsumerConfig<TMessage>
    {
        /// <summary>
        /// Consumer name, used in worker name
        /// </summary>
        string Name { get; }
        int WorkerCount { get; }
        /// <summary>
        /// PrefetchCount value is used to specify how many messages are being sent at the same time.
        /// Messages are cached by the RabbitMQ client library (in the consumer) until processed. 
        /// All pre-fetched messages are invisible to other consumers and are listed as unacked messages in the RabbitMQ management interface.
        /// </summary>
        ushort PrefetchCount { get; }
        RabbitQueue Queue { get; }
        bool RequeueOnFailed { get; }
    }

    public interface IConsumer<TMessage>: IStatusGeneric
        where TMessage : class
    {
        Task ConsumeAsync(TMessage message, CancellationToken cancellationToken);
    }
}
