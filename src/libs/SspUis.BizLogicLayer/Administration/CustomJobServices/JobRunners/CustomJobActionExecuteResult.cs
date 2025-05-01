using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public class CustomJobActionExecuteResult
    {
        public string ReturnData { get; set; }
        public string UserMessage { get; set; }
        public bool FromCache { get; set; }
    }
}
