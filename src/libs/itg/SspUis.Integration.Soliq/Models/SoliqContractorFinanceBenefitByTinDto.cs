using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqContractorFinanceBenefitByTinDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SoliqContractorFinanceBenefitByTinDataDto Data { get; set; } = new();
    }

    public class SoliqContractorFinanceBenefitByTinDataDto
    {
        [JsonProperty("tin")]
        public int Tin { get; set; }

        [JsonProperty("name")]
        public string Name{ get; set; }

        [JsonProperty("netIncome")]
        public decimal? NetIncome{ get; set; }
    }
}
