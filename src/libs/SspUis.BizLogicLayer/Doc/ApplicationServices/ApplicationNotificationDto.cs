using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Doc.ApplicationServices
{
    public class ApplicationNotificationDto
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
