using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionCertificateSortFilterOptions : SortFilterPageOptions
{
    public string ContractorInn { get; set; }
    public int? RegionId { get; set; }
    public int? StatusId { get; set; }
    public int? DistrictId { get; set; }
    public long? ContractorId { get; set; }
}
