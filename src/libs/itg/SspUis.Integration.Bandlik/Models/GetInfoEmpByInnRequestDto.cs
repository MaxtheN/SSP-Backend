using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public class GetInfoEmpByInnRequestDto: BandlikRequestDto
    {
        [JsonProperty("params")]
        public GetInfoEmpByInnParams Params { get; set; }

    }
    public class GetInfoEmpByInnParams
    {
        [JsonProperty("query")]
        public GetInfoEmpByInnQuery Query { get; set; }
    }

    public class GetInfoEmpByInnQuery
    {   
        [JsonProperty("tin")]
        public string Tin { get; set; }
    }

}
