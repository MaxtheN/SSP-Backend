using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class ImtiyozDataByInnResponseDto
    {
        //[JsonProperty("success")]
        public bool Success { get; set; }

        //[JsonProperty("reason")]
        public string Reason { get; set; }

        //[JsonProperty("data")]
        public List<ImtiyozDataByInnDataDto> Data { get; set; }
    }
    public class ImtiyozDataByInnDataDto
    {
        //[JsonProperty("tin")]
        public string Tin { get; set; }

        //[JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("lgotaId")]
        public int LgotaId { get; set; }

        //[JsonProperty("summa")]
        public double Summa { get; set; }

        //[JsonProperty("cnt")]
        public int Cnt { get; set; }
    }
}
