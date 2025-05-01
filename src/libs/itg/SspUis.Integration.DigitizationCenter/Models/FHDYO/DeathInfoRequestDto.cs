using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Models.FHDYO
{
    public class DeathInfoRequestDto
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "111";
        
        [JsonProperty("pin")]
        public string Pin { get; set; }
    }
}
