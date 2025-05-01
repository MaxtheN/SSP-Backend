using System;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ContractorCategoryDtoFilter
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public int? RegionId { get; set; }
        public bool ByRegion{ get; set; } = false;

        public long? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;
    }
}
