using WEBASE.Models;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices;

public class PrtnCreditDemandSortFilterOptions : SortFilterPageOptions
{
    public string? ContractorInn { get; set; }
    public long? CertificateId { get; set; }
    public long? ApplicationId { get; set; }
    public int? RegionId { get; set; }
    public int? StatusId { get; set; }
    public int? DistrictId { get; set; }
    public int? PrtnContractTypeId { get; set; }
}
