namespace SspUis.BizLogicLayer.MemshipApplicationServices;

public class MemshipApplicationSortFilterOptions : DocumentSortFilterOptions
{
    public string ContractorInn { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? StatusId { get; set; }
    public int? ContractorCategoryId { get; set; }
    public bool? IsPinfl { get; set; }
    public int? ContractorActivityTypeId { get; set; }
    public int? OpfId { get; set; }
}
