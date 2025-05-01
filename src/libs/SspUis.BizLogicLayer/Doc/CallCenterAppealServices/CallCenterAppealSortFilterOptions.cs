using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class CallCenterAppealSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? RegionId { get; set; }
    public string? PersonFullName { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
    public string? ContractorInn { get; set; }
    public int? DistrictId { get; set; }
    public int? AppealFormatTypeId { get; set; }
    public int? AppealTypeId { get; set; }
    public int? AppealTypeArriveId { get; set; }
    public int? AppealDescriptionId { get; set; }
}
