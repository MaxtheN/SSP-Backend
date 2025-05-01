using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Claim.Doc.ApplicationForCourtServices;

public class CheckAmountFromBankDto
{
    public decimal? MainDebtFromBank { get; set; }
    public decimal? CalculedPenaltyFromBank { get; set; }
    public decimal? PenaltyFromBank { get; set; }
    public decimal? PercentFromBank { get; set; }
    public decimal? CurrentPrincipalInterestFromBank { get; set; }
    public decimal? CurrentInterestRateFromBank { get; set; }
    public decimal? OtherDebtRePaymentFromBank { get; set; }
    public DateOnly? DateFromBank { get; set; } 
    [JsonIgnore]
    public decimal? Summ { get; set; }


    public decimal? MainDebtFromSsp { get; set; }
    public decimal? CalculedPenaltyFromSsp { get; set; }
    public decimal? PenaltyFromSsp { get; set; }
    public decimal? PercentFromSsp { get; set; }
    public decimal? CurrentPrincipalInterestFromSsp { get; set; }
    public decimal? CurrentInterestRateFromSsp { get; set; }
    public decimal? OtherDebtRePaymentFromSsp { get; set; }
    public DateOnly? DateFromSsp{ get; set; }
    public string? BankName { get; set; }


}
