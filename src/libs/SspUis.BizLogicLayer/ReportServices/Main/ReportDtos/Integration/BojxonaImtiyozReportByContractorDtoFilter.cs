namespace SspUis.BizLogicLayer.ReportServices
{
    public class BojxonaImtiyozReportByContractorDtoFilter
    {
        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;
        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;
        public long? ContractorId { get; set; }
        public string? ContractorInn { get; set; }
        public bool ByContractor { get; set; } = false;
        public int? Year { get; set; }
        public int? LanguageId { get; set; }
        public int? ContarctTypeId { get; set; }
        public bool ByContactType { get; set; }
        public bool HasCertificate { get; set; } = true;
        public int? Tab { get; set; }
    }
}
