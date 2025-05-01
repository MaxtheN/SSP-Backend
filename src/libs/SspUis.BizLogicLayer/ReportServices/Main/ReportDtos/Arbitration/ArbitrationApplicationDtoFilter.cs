using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ArbitrationApplicationDtoFilter
    {
        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;

        public int? ContractorId { get; set; }
        public bool ByContractor { get; set; } = false;
    }
}