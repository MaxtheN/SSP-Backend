using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestSortFilterOption : SortFilterPageOptions
{
    public int? OrganizationId { get; set; }
    public int? StatusId { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
    public string? ContractorInn { get; set; }
    public int? DistrictId { get; set; }
    public int? RegionId { get; set; }
    public string? Address { get; set; }
    public string PhoneNumber { get; set; }
}
