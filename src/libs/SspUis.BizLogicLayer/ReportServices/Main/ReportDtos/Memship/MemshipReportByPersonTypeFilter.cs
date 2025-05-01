using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Memship
{
    public class MemshipReportByPersonTypeFilter
    {
        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;
        public long? ContractorId { get; set; }
        public bool ByContractor { get; set; }
        public bool? IsOld { get; set; }

        public DateOnly? FromDocDate { get; set; }
        public DateOnly? ToDocDate { get; set; }
    }
}