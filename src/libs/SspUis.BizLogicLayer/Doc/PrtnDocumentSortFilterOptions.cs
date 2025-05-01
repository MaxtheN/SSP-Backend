using System;

namespace SspUis.BizLogicLayer
{
    public class PrtnDocumentSortFilterOptions : DocumentSortFilterOptions
    {
        public string ContractorInn { get; set; }
        public string? Inn { get; set; }
        public int? RegionId { get; set; }
        public int? StatusId { get; set; }
        public int? DistrictId { get; set; }
        public long? MfyId { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public bool? HasCertificate { get; set; }
        public string OkedCode { get; set; }
        public int? ExpireDay { get; set; }
        public bool IsTotalSuccessPost { get; set; } = true;

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? OkedTypeId { get; set; }
        public bool ByRegion { get; set; } = false;
        public bool ByDistrict { get; set; } = false;

        public long? ContractorId { get; set; }
        public bool ByContractor { get; set; } = false;
    }
}
