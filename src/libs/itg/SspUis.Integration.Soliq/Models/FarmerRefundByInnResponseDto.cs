using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class FarmerRefundByInnResponseDto
    {
        //[JsonProperty("success")]
        public bool Success { get; set; }

        //[JsonProperty("reason")]
        public string Reason { get; set; }

        //[JsonProperty("data")]
        public FarmerRefundByInnDataDto Data { get; set; }
    }
    public class FarmerRefundByInnDataDto
    {
        //[JsonProperty("summa")]
        public decimal Summa { get; set; }

       // [JsonProperty("companyTin")]
        public string CompanyTin { get; set; }

        //[JsonProperty("applicationCount")]
        public int ApplicationCount { get; set; }
    }
}
