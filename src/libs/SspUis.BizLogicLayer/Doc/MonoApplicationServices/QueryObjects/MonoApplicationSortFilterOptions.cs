using System;

namespace SspUis.BizLogicLayer.MonoApplicationServices;

public class MonoApplicationSortFilterOptions : DocumentSortFilterOptions
{
    public string ContractorInn { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? MonoRegionId { get; set; }
    public int? MonoDistrictId { get; set; }
    public int? StatusId { get; set; }
}
