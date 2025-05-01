using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Notify.Sms
{
    public class SmsProviderConfig
    {
        public string ProviderUrl { get; set; }
        public string HttpMethod { get; set; }
        public string HttpBody { get; set; }
        public string Authorization { get; set; }
        public List<MessageTemplateConf> MessageTemplate { get; set; } = new();
        public class MessageTemplateConf
        {
            public string Text { get; set; }
            public string Action { get; set; }
        }
    }
}
