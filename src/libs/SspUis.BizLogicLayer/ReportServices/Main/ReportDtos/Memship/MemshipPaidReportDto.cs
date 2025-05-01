namespace SspUis.BizLogicLayer.ReportServices;

public class MemshipPaidReportDto
{
    public string Region { get; set; }
    public int? RegionId { get; set; }
    public string RegionOrderCode { get; set; }

    public string District { get; set; }
    public int? DistrictId { get; set; }
    public string DistrictOrderCode { get; set; }

    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }

    public int SentMemshipApplicationCount { get; set; }
    public int AcceptedMemshipApplicationCount { get; set; }
    public int CreatedMemshipContractCount { get; set; }
    public int SigningMemshipContractCount { get; set; }
    public int SignedMemshipContractCount { get; set; }
    public int MemshipCertificateCount { get; set; }
    public decimal MemshipCertificateContributionBXM { get; set; }
    public decimal MemshipCertificateContributionAmount { get; set; }
    public decimal MemshipCertificateRevenueAmount { get; set; }
    public decimal MemshipCertificateIndebtednessAmount { get; set; }
}
