using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class BusinessActivityTypeReportByRegionFilter
    {
        public bool ByRegion { get; set; } = false;
        public bool ByDistrict { get; set; } = false;
        public int? RegionId { get; set; }
        //public int? DistrictId { get; set; }
    }
}