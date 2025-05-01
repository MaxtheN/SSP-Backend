using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WEBASE.Integration.Manuals.Models
{
    public class Citizenship
    {
        [JsonProperty("wbcode")]
        public string WbCode { get; set; }
        [JsonProperty("displayname")]
        public string DisplayName { get; set; }
        [JsonProperty("gspid")]
        public int GspId { get; set; }
        [JsonProperty("gnkid")]
        public string GnkId { get; set; }
    }
}
