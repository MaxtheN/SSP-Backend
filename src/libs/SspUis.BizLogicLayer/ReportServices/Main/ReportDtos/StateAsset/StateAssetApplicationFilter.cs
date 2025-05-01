using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class StateAssetApplicationFilter
    {
        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;
    }
}