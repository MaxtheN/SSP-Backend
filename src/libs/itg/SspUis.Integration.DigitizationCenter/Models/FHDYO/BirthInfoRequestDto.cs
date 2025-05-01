using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Models.FHDYO
{
    public class BirthInfoRequestDto
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "111";
        [JsonProperty("cert_series")]
        public string CertSeries { get; set; }

        [JsonProperty("cert_number")]
        public string CertNumber { get; set; }
    }
}
