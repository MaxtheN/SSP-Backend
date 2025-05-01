using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class GetSrvServiceInfoRequestDto
    {
        public bool IsFree { get; set; }
        public bool ByRegion { get; set; }
        public int? RegionId { get; set; }
        public bool ByDistrict { get; set; }
        public bool ByContractor { get; set; }
        public int? DistrictId { get;set; }
        public DateOnly? FromDocDate { get; set; }
        public DateOnly? ToDocDate { get; set; }
    }
}
