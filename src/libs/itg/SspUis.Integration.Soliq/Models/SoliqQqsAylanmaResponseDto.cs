using Newtonsoft.Json;

namespace SspUis.Integration.Soliq.Models;

public class SoliqQqsAylanmaResponseDto
{
    public bool Success { get; set; }
    public string Reason { get; set; }
    public SoliqQqsAylanmaData Data { get; set; }

}

public class SoliqQqsAylanmaData
{
    public long Tin { get; set; }
    public string Name { get; set; }
    public decimal NetIncomeWithoutVat { get; set; }
    public decimal VatSum { get; set; }
}
 