using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.OnlineMahalla
{
    public class MahallaApplicationStatusUpdateRequestDto
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
