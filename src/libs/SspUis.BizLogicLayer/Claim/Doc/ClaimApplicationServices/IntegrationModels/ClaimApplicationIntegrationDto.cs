using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.ClaimApplicationServices;

public class ClaimApplicationIntegrationRequestDto
{ 
    public int OrganizationId { get; set; }
    public string? ContractIdentificationNumber { get; set; }
    public ClaimApplicationIntegrationDto Document { get; set; } = new();
}

public class ClaimApplicationIntegrationDto
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]   
    public int ClaimApplicationTypeId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ClaimThemeId { get; set; }
    public string Details { get; set; }
    public decimal? MainDebt { get; set; }
    public decimal? CalculedPenalty { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Percent { get; set; }
    public decimal? CurrentPrincipalInterest { get; set; }
    public decimal? CurrentInterestRate { get; set; }
    public decimal? OtherDebtRepayment { get; set; }
    public long? PrevApplicationId { get; set; }
    public int CurrencyId { get; set; }
    public string? BankBranchName { get; set; }
    public string? BankResponsiblePerson { get; set; }
    public List<ClaimsApplicationIntegrationFileDto> Files { get; set; } = new();
    public List<ClaimApplicationIntegrationTableDto> Tables { get; set; } = new();
}


public class ClaimsApplicationIntegrationFileDto
{
    public string FileAsBase64 { get; set; }
    public string FileExtension { get; set; }
}
public class ClaimApplicationIntegrationTableDto
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ClaimResponsibleTypeId { get; set; }
    [LocalizedRequired]
    [LocalizedMinLength(9)]
    [LocalizedStringLength(14)]
    public string InnOrPinfl { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string FullName { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string Address { get; set; }
    [LocalizedRequired]
    [LocalizedMinLength(12)]
    [LocalizedStringLength(50)]
    public string PhoneNumber { get; set; }
}
