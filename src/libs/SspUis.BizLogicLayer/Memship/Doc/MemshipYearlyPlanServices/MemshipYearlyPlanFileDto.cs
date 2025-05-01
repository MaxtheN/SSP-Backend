using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Memship
{
    public class MemshipYearlyPlanFileDto : MemshipYearlyPlanFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
