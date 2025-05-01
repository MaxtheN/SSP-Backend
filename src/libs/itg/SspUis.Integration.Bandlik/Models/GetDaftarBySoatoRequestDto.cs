using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public class GetDaftarBySoatoRequestDto: BandlikRequestDto
    {
        [JsonProperty("params")]
        public GetDaftarBySoatoParams Params { get; set; }

    }
    public class GetDaftarBySoatoParams
    {
        [JsonProperty("body")]
        public GetDaftarBySoatoQuery Body { get; set; }
    }

    public class GetDaftarBySoatoQuery
    {
        [JsonProperty("soato")]
        public int Soato { get; set; }

        [JsonProperty("enter_date")]
        public string EnterDate { get; set; }

        [JsonProperty("service")]
        public int Service { get; set; }
    }

}
