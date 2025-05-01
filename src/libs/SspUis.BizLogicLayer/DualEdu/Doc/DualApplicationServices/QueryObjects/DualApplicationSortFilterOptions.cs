namespace SspUis.BizLogicLayer.DualApplicationServices;

public class DualApplicationSortFilterOptions : DocumentSortFilterOptions
{
    public string ContractorInn { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
}