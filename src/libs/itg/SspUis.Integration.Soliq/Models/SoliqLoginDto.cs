using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqLoginDto
    {
        [JsonProperty("username")]
        public string UserName { get; set; } = null!;
        [JsonProperty("password")]
        public string Password { get; set; } = null!;
    }
}
