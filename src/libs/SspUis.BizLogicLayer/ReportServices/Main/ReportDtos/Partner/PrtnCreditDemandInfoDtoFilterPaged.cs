namespace SspUis.BizLogicLayer.ReportServices;

public class PrtnCreditDemandInfoDtoFilter
{
    public int? ContractTypeId { get; set; }

    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;

    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; } = false;

    public long? ContractorId { get; set; }
    public string? ContractorInn { get; set; }
    public bool ByContractor { get; set; } = false;

}