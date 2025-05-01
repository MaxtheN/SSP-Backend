using Newtonsoft.Json;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.DataLayer.Repositories
{
    public class MonoApplicationBandlikResultDlDto
    {
        [LocalizedRequired]
        [JsonProperty("applicationId")]
        public long ApplicationId { get; set; }

        [LocalizedRequired]
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("subsidy_amount")]
        public string SubsidyAmount { get; set; }

        [JsonProperty("responsibleFio")]
        public string ResponsibleFio { get; set; }

        [JsonProperty("responsiblePhone")]
        public string ResponsiblePhone { get; set; }

        [JsonProperty("reject_reason")]
        public string RejectReason { get; set; }

    }
}
