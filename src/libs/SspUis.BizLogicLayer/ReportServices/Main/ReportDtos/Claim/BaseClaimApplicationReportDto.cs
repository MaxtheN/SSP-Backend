namespace SspUis.BizLogicLayer.ReportServices;

public class BaseClaimApplicationReportDto : ClaimApplicationReportsDtoFilter
{
    public string Region { get; set; }
    public string Organisation { get; set; }
    public int? OrganisationId { get; set; }
    public string RegionOrderCode { get; set; }

    public string District { get; set; }
    public string DistrictOrderCode { get; set; }

    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPhoneNumber { get; set; }

    public string ClaimApplicationType { get; set; }

    public long TotalApplicationAmountInArea { get; set; }
}
