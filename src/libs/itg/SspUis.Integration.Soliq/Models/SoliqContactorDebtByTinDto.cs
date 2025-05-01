using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqContractorDebtByTinDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SoliqContractorDebtByTinDataDto Data { get; set; } = new();
    }

    public class SoliqContractorDebtByTinDataDto
    {

        [JsonProperty("tin")]
        public int Tin { get; set; }

        [JsonProperty("send_id")]
        public string SendId { get; set; }

        [JsonProperty("send_date")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly SendDate { get; set; }

        [JsonProperty("tax_debt")]
        public decimal? TaxDebt { get; set; }
    }
}
