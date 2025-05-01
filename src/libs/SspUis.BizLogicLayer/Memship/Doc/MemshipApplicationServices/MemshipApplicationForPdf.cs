using Newtonsoft.Json;
using System;

namespace SspUis.BizLogicLayer;
public class MemshipApplicationForPdf
{
    public string? lang { get; set; } = "uz-cyrl";
    public Guid Id2 { get; set; }
    public string? DocOn { get; set; }
    public string? Director { get; set; }
    public string? Region { get; set; }
    public string? DocNumber { get; set; }
    public string? ContractorFullName { get; set; }
    public string? ContractorDirector { get; set; }
    public string? RegionName { get; set; }
    public string? ContractorDistrict { get; set; }
    public string? DistrictName { get; set; }
    public string? ContractorWorkPhoneNumber { get; set; }
    public string? ContractorMobilePhoneNumber { get; set; }
    public string? ContractorFaks { get; set; }
    public string? ContractorAdditionalPhoneNumber { get; set; }
    public string? ContractorEmail { get; set; }
    public string? ContractorWebSite { get; set; }
    public string? ContractorSkype { get; set; }
    public string? ContractorFacebook { get; set; }
    public string? ContractorTelegram { get; set; }
    public string? Inn { get; set; }
    public string? OkedCode { get; set; }
    public string? ActivityTypeFullName { get; set; }
    public string? ContractorCategoryFullName { get; set; }
    public string? EmployeesCount { get; set; }
    public string? BankName { get; set; }
    public string? BankCode { get; set; }
    public string? SettlementAccount { get; set; }
    public string? YearlyEarnings { get; set; }
    public string? YearlyTaxes { get; set; }
    public string? YearlyExport { get; set; }
    public string? YearlyImport { get; set; }
    public string? YearlyManufacture { get; set; }
    public string? RegDocOn { get; set; }
    public string? RegDocNumber { get; set; }
    public string? NeedChamberService { get; set; }
}
