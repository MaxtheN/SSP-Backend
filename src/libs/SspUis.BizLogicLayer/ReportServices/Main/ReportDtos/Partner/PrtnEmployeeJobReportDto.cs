namespace SspUis.BizLogicLayer.ReportServices;

public class PrtnEmployeeJobReportDto
{
    public int? PrtnContractTypeId { get; set; }
    public string? PrtnContractType { get; set; }

    public int? RegionId { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }

    public int? DistrictId { get; set; }
    public string District { get; set; }

    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPhoneNumber { get; set; }

    public int PrtnCertificateCount { get; set; }
    public int TotalPlannedJobs { get; set; }
    public int TodayPlan { get; set; }
    public decimal ActualEmployeesCount { get; set; }
    public decimal TotalEmployeesCount { get; set; }
    public decimal InitialEmployeesCount { get; set; }
    public decimal PercentageOfCompletedPlan { get; set; }
    public decimal PercentageGrowth { get; set; }
}