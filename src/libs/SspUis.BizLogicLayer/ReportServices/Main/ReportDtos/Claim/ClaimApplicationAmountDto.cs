namespace SspUis.BizLogicLayer.ReportServices;

public class ClaimApplicationAmountDto
{
    public int? RegionId { get; set; }
    public string Region { get; set; }
    public string RegionOrderCode { get; set; }

    public int? DistrictId { get; set; }
    public string District { get; set; }
    public string DistrictOrderCode { get; set; }

    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    //public string ContractorPinfl { get; set; }
    public string ContractorInn { get; set; }

    public decimal? TotalClaimApplicationAmount { get; set; }
    public decimal? TotalChargedAmount { get; set; }
    public int? ClaimThemeId { get; set; }
    public string? ClaimTheme { get; set; }
}