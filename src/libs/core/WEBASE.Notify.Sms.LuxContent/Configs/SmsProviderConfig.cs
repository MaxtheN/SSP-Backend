using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Notify.Sms.LuxContent
{
    public class LuxContentSmsProviderConfig
    {
        public string ProviderUrl { get; set; }
        public string Method { get; set; }
        public string Service { get; set; }
        public string User { get; set; }
        public string Key { get; set; }
        public string MessageTemplate { get; set; } = "{0}";
    }
}
