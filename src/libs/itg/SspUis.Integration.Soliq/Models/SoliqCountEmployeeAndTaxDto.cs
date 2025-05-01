using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class ЫoliqCountEmployeeAndTaxResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public SoliqCountEmployeeAndTaxDto Data { get; set; } = new();
    }
    public class SoliqCountEmployeeAndTaxDto
    {
        public string Tin { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public int MonthlyNumberEmployees { get; set; }
        public long PaymentTax { get; set; }
    }
}
