using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Doc.StateAssetApplicationServices
{
    public class UpdateDavAktivStatusDto
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
