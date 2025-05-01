using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public class GetStatisticByInnRequestDto: BandlikRequestDto
    {
        [JsonProperty("params")]
        public GetStatisticByInnParams Params { get; set; }

    }
    public class GetStatisticByInnParams
    {
        [JsonProperty("query")]
        public GetStatisticByInnQuery Query { get; set; }
    }

    public class GetStatisticByInnQuery
    {
        [JsonProperty("tin")]
        public string Tin { get; set; }
    }

}
