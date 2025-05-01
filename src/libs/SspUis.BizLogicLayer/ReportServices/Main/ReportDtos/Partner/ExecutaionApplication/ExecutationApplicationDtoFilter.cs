using System;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ExecutationApplicationDtoFilter
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;

        //public int? ContractorId { get; set; }
        public bool ByContractor { get; set; } = false;
    }
}