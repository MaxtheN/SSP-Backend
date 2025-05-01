using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ImportUserDlDto
    {
        [LocalizedRequired]
        [JsonProperty("Orginn")]
        public string OrganizationInn { get; set; }

        [LocalizedRequired]
        [JsonProperty("Phone_number")]
        public string DirectorPhoneNumber{ get; set; }
        [LocalizedRequired]
        [JsonProperty("Date_of_birth")]
        public DateOnly DirectorBirthOn { get; set; }
        [LocalizedRequired]
        [JsonProperty("Pasport_serie")]
        public string DirectorPassportSeria { get; set; }
        [LocalizedRequired]
        [JsonProperty("Pasport_number")]
        public string DirectorPassportNumber { get; set; }

    }
}
