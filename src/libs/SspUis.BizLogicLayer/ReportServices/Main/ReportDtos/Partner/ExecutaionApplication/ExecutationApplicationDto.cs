namespace SspUis.BizLogicLayer.ReportServices;

public class ExecutationApplicationDto
{
    public int? RegionId { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }

    public int? DistrictId { get; set; }
    public string District { get; set; }

    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }

    public long? TotalContractorCount { get; set; }
    public int? TotalPrtnNewVacanseCount { get; set; }

    public int? PrtnContractorCountExpired { get; set; }
    public int? PrtnNewVacanseCountExpired { get; set; }

    public int? ContractorCountFromSoliq { get; set; }
    public int? NewVacanseCountFromSoliq { get; set; }
    public decimal? AvarageSalaryFromSoliq { get; set; }

    public int? ExecutationContractorCountExpired { get; set; }
    public int? ExecutationNewVacanseCountExpired { get; set; }
    public decimal? ExecutationAvarageSalary { get; set; }

    public int? CreditFromBankCount { get; set; }
    public decimal? CreditFromBankAmount { get; set; }

    public int? WarrantyProvidedCount { get; set; }
    public decimal? WarrantyProvidedAmount { get; set; }

    public int? MolMulkSoliqCount { get; set; }
    public decimal? MolMulkSoliqAmount { get; set; }

    public int? DaromadSoliqCount { get; set; }
    public decimal? DaromadSoliqAmount { get; set; }

    public int? Soliq50FoizMiqdoriCount { get; set; }
    public decimal? Soliq50FoizMiqdoriAmount { get; set; }

    public int? QQSCount { get; set; }
    public decimal? QQSAmount { get; set; }

    public int? SoliqFoizsizCount { get; set; }
    public decimal? SoliqFoizsizAmount { get; set; }

    public int? TotalGreenRoadContractorCount { get; set; }

    public decimal? SumFromBojxona { get; set; }
    public decimal? ContractorCountFromBojxona { get; set; }
}