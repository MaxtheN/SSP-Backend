
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices.Main;

public class ContractorBankCreditReportDtoFilter
{
    public string? ContractorInn { get; set; }
    public int? Year { get; set; }
    public string? BankMfo { get; set; }
    public bool ByRegion { get; set; } = false;
    public bool ByDistrict { get; set; } = false;
    public bool ByBank { get; set; } = false;
    public bool ByBankCode { get; set; } = false;
    public bool ByContractor { get; set; } = false;
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
}

public class ContractorBankCreditReportDtoFilterPageOptions: SortFilterPageOptions
{
    public string? ContractorInn { get; set; }
    public int? Year { get; set; }
    public string? BankMfo { get; set; }
    public bool ByRegion { get; set; } = false;
    public bool ByBank { get; set; } = false;
    public bool ByBankCode { get; set; } = false;
    public bool ByDistrict { get; set; } = false;
    public bool ByContractor { get; set; } = false;

    public int? RegionId { get; set; }

    public int? DistrictId { get; set; }
}