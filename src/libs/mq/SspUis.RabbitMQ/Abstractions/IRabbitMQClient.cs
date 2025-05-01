using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.RabbitMQ.Abstractions
{
    public interface IRabbitMQClient
    {
        string AppName { get; }
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
        IModel CreateModel(string queueName);
    }
}
