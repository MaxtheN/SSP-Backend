using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class BusinessActivityTypeReprotFilter
    {
        public bool ByBank { get; set; } = false;   
        public int? BankCodeId { get; set; }
     
    }
}