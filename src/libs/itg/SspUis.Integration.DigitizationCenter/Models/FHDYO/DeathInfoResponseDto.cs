using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter.Models.FHDYO
{
   
    public class DeathInfoResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("result_code")]
        public int ResultCode { get; set; }

        [JsonProperty("result_message")]
        public string ResultMessage { get; set; }

        [JsonProperty("items")]
        public List<DeathInfoResponseDto> Items { get; set; }
    }
    public class DeathInfoResponseDto
    {
        [JsonProperty("pnfl")]
        public string Pnfl { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("patronym")]
        public string Patronym { get; set; }

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        [JsonProperty("gender_code")]
        public int GenderCode { get; set; }

        [JsonProperty("doc_num")]
        public string DocNum { get; set; }

        [JsonProperty("doc_date")]
        public string DocDate { get; set; }

        [JsonProperty("branch")]
        public int Branch { get; set; }

        [JsonProperty("cert_series")]
        public string CertSeries { get; set; }

        [JsonProperty("cert_number")]
        public string CertNumber { get; set; }

        [JsonProperty("cert_birth_date")]
        public string CertBirthDate { get; set; }

        [JsonProperty("f_family")]
        public string FFamily { get; set; }

        [JsonProperty("f_first_name")]
        public string FFirstName { get; set; }

        [JsonProperty("f_patronym")]
        public string FPatronym { get; set; }

        [JsonProperty("f_birth_day")]
        public string FBirthDay { get; set; }

        [JsonProperty("f_pnfl")]
        public string FPnfl { get; set; }

        [JsonProperty("m_family")]
        public string MFamily { get; set; }

        [JsonProperty("m_first_name")]
        public string MFirstName { get; set; }

        [JsonProperty("m_patronym")]
        public string MPatronym { get; set; }

        [JsonProperty("m_birth_day")]
        public string MBirthDay { get; set; }

        [JsonProperty("m_pnfl")]
        public string MPnfl { get; set; }
    }
}
