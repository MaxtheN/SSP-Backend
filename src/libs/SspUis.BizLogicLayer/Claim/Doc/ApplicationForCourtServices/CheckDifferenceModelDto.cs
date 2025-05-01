using Newtonsoft.Json;
using System;

namespace SspUis.BizLogicLayer.Claim;
public class CheckDifferenceModelDto
{
    public int? BalanceRemainder { get; set; }
    public int? OverdueRemainder { get; set; }
    public int? PercentsRemainder { get; set; }
    public int? PenaltiesRemainder { get; set; }
    public DateOnly? DateFromBank { get; set; }

    public decimal? MainDebt { get; set; }
    public decimal? CalculedPenalty { get; set; }
    public decimal? Penalty { get; set; }
    public decimal? Percent { get; set; }
    public decimal? CurrentPrincipalInterest { get; set; }
    public decimal? CurrentInterestRate { get; set; }
    public decimal? OtherDebtRepayment { get; set; }
    public DateOnly? ApplicaitonDocOn { get; set; }
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int Summ { get; set; }
}
