using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.Integration.OnlineMahalla
{
    public class OnlineMahallaRequestDto
    {
        [JsonProperty("obl_soato")]
        public string OblSoato { get; set; }

        [JsonProperty("area_soato")]
        public string AreaSoato { get; set; }

        [JsonProperty("district_id")]
        public long DistrictId { get; set; }

        [JsonProperty("org_tin")]
        public long OrgTin { get; set; }

        [JsonProperty("org_name")]
        public string OrgName { get; set; }

        [JsonProperty("contract_type")]
        public int ContractType { get; set; }

        [JsonProperty("staff_count")]
        public int StaffCount { get; set; }

        [JsonProperty("application_num")]
        public string ApplicationNum { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("application_date")]
        public DateTime ApplicationDate { get; set; }
        [JsonProperty("org_phone")]
        public string OrgPhoneNumber { get; set; }
        [JsonProperty("graphs")]
        public List<OnlineMahallaRequestGraphDto> Graphs { get; set; } = new();
    }

    public class OnlineMahallaRequestGraphDto
    {
        [JsonProperty("year_in")]
        public int YearIn { get; set; }
        [JsonProperty("month_in")]
        public int MonthIn { get; set; }
        [JsonProperty("new_vacancies_count")]
        public int NewVacanciesCount { get; set; }
    }
}
