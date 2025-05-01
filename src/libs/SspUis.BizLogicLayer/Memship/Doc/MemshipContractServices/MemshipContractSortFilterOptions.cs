using System;
using WEBASE.Models;
namespace SspUis.BizLogicLayer.Memship;

public class MemshipContractSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? OrganizationId { get; set; }
    public int? MemshipContractTypeId { get; set; }
    public long? MemshipApplicationId { get; set; }
    public string? ContractorInn { get; set; }
    public bool? IsPinfl { get; set; }
    public long? ContractorSettlementAccountId { get; set; }
    public long? OrganizationSettlementAccountId { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }
    public bool? IsOld { get; set; } = false;
    public int? ContractorCategoryId { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public bool? IsConfirmed { get; set;}
    public bool? HasCertificate { get; set; }
    public bool? HasCertificateCanceled { get; set; }
    public int? OpfId { get; set; }
}
