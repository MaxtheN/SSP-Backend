namespace SspUis.BizLogicLayer.ReportServices
{
    public class ClaimApplicationReportsDtoFilter
    {
        public int? ClaimApplicationTypeId { get; set; }

        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;

        public long? ContractorId { get; set; }
        public bool IsSsp { get; set; }
        public bool ByContractor { get; set; } = false;
    }
}