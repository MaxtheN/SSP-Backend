
using System.Text.Json.Serialization;

namespace SspUis.Integration.XalqBank.Models;

public class ChekAmountResponse
{
    [JsonPropertyName("mainDebt")]
    public decimal MainDebt { get; set; }
    [JsonPropertyName("calculedPenalty")]
    public decimal CalculedPenalty { get; set; }
    [JsonPropertyName("penalty")]
    public decimal Penalty { get; set; }
    [JsonPropertyName("percent")]
    public decimal Percent { get; set; }
    [JsonPropertyName("currentPrincipalInterest")]
    public decimal CurrentPrincipalInterest { get; set; }
    [JsonPropertyName("currentInterestRate")]
    public decimal CurrentInterestRate { get; set; }
    [JsonPropertyName("otherDebtRePayment")]
    public decimal OtherDebtRePayment { get; set; }
}
