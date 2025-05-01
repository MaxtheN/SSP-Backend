using System;

namespace SspUis.BizLogicLayer.ReportServices;

public class ClaimApplicationAmountDtoFilter
{
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;

    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; } = false;

    public int? ClaimThemeId { get; set; }

    //public string? Pinfl { get; set; }

    public int? ContractorId { get; set; }
    public string? ContractorInn { get; set; }
    public bool ByContractor { get; set; } = false;

    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}