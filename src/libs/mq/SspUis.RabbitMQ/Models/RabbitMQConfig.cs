using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.RabbitMQ.Models
{
    public class RabbitMQConfig
    {
        public string AppName { get; set; }
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";

        public void Init(RabbitMQConfig config)
        {
            Host = config.Host;
            Port = config.Port;
            VirtualHost = config.VirtualHost;
            UserName = config.UserName;
            Password = config.Password;
        }
    }
}
