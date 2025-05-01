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
    public class CheckPersonFromGspDlDto
    {
        [LocalizedRequired]
        [JsonProperty("day")]
        public int Day { get; set; }

        [LocalizedRequired]
        [JsonProperty("month")]
        public int Month { get; set; }

        [LocalizedRequired]
        [JsonProperty("year")]
        public int Year { get; set; }

        [LocalizedRequired]
        [JsonProperty("seria")]
        public string Seria { get; set; }

        [LocalizedRequired]
        [JsonProperty("number")]
        public string Number { get; set; }

        //[LocalizedRequired]
        //[JsonProperty("Orginn")]
        //public string OrganizationInn { get; set; }
        //
        //[LocalizedRequired]
        //[JsonProperty("Phone_number")]
        //public string DirectorPhoneNumber{ get; set; }
        //[LocalizedRequired]
        //[JsonProperty("Date_of_birth")]
        //public DateTime DirectorBirthOn { get; set; }
        //[LocalizedRequired]
        //[JsonProperty("Pasport_serie")]
        //public string DirectorPassportSeria { get; set; }
        //[LocalizedRequired]
        //[JsonProperty("Pasport_number")]
        //public string DirectorPassportNumber { get; set; }
        //
        //[LocalizedRequired]
        //[JsonProperty("Deputy_phone_number")]
        //public string DeputyPhoneNumber { get; set; }
        //[LocalizedRequired]
        //[JsonProperty("Deputy_date_of_birth")]
        //public DateTime DeputyBirthOn { get; set; }
        //[LocalizedRequired]
        //[JsonProperty("Deputy_pasport_serie")]
        //public string DeputyPassportSeria { get; set; }
        //[LocalizedRequired]
        //[JsonProperty("Deputy_pasport_number")]
        //public string DeputyPassportNumber { get; set; }

    }
}
