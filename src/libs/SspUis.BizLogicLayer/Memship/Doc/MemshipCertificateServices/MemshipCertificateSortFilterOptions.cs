using System;
using WEBASE.Models;
namespace SspUis.BizLogicLayer;

public class MemshipCertificateSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
    public DateOnly? FromExpireOn { get; set; }
    public DateOnly? ToExpireOn { get; set; }
    public DateOnly? Less1MonthLeft { get; set; }
    public string? ContractorInn { get; set; }
    public bool? IsOld { get; set; } = false;
    public bool? IsPinfl { get; set; }
    public int? ContractorCategoryId { get; set; }
    public int? ContractorOkedId { get; set; }
    public int? MemshipContractTypeId { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int? OpfId { get; set; }
    public bool ByExpireOn { get; set; } = false;
}