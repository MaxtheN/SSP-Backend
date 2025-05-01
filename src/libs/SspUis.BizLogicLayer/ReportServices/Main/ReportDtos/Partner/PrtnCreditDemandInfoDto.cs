namespace SspUis.BizLogicLayer.ReportServices;

public class PrtnCreditDemandInfoDto
{
    public string ContractType { get; set; }
    public int ContractTypeId { get; set; }

    public int? RegionId { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }

    public int? DistrictId { get; set; }
    public string District { get; set; }

    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPhoneNumber { get; set; }

    public double TotalDocCount { get; set; }
    public double TotalProjectCost { get; set; }
    public double TotalOwnInvestment { get; set; }
    public double TotalForeignInvestment { get; set; }
    public double TotalPrivillageBankCredit { get; set; }
}