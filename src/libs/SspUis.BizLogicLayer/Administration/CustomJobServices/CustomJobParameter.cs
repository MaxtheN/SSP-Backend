using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.CustomJobServices
{
    public class CustomJobParameter
    {
        public long CustomJobId { get; set; }

        public string UserName { get; set; }  
        public int JobTypeId { get; set; }  
    }
}
