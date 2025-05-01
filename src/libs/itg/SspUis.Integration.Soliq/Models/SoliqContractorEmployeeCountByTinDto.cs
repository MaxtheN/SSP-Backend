using Newtonsoft.Json;
using SspUis.Core;
using System.Net;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqContractorEmployeeCountByTinDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SoliqContractorEmployeeCountByTinDataDto Data { get; set; } = new();
    }

    public class SoliqContractorEmployeeCountByTinDataDto
    {
        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("monthlyNumberEmployees")]
        public int MonthlyNumberEmployees { get; set; }

        [JsonProperty("paymentTax")]
        public decimal? PaymentTax { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
