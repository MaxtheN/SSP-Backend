using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class AdditionalAgreementSortFilterOption :SortFilterPageOptions
{
    public string ContractorInn { get; set; }
    public long? RegionId { get; set; }
    public long? DistrictId { get; set; }
    public int? StatusId { get; set; }
    public DateOnly? FromDocDate { get; set; }
    public DateOnly? ToDocDate { get; set; }



}

