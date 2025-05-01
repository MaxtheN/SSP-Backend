using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bandlik.Models
{
    public record BandlikResponseDto<T>
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
    public class BandlikRequestMonoPost:BandlikRequestDto 
    {
        [JsonProperty("params")]
        public BandlikRequestMonoPostParams Params { get; set; }
    }
    public class BandlikRequestMonoPostParams
    {
        [JsonProperty("body")]
        public BandlikRequestMonoPostDto Body { get; set; }
    }
    public class BandlikRequestMonoPostDto
    {
        [JsonProperty("type")]
        public int TypeId { get; set; }
        [JsonProperty("application_id")]
        public long ApplicationId { get; set; }
        [JsonProperty("company_tin")]
        public int ContractorInn { get; set; } 
        [JsonProperty("org_name")]
        public string ContractorName { get; set; } 
        [JsonProperty("org_address")]
        public string ContractorAdress { get; set; } 
        [JsonProperty("org_city_soato")]
        public int ContractorSoato { get; set; } 
        [JsonProperty("phone")]
        public string ContractorPhone { get; set; } 
        [JsonProperty("fax")]
        public string ContractorFax { get; set; } 
        [JsonProperty("email")]
        public string ContractorEmail { get; set; } 
        [JsonProperty("registration_number")]
        public string RegistrationNumber { get; set; } 
        [JsonProperty("registration_date")]
        public string RegistrationDate { get; set; }
        [JsonProperty("registrated_from")]
        public string RegistrationFrom { get; set; } 
        [JsonProperty("org_director")]
        public string ContractorDirector { get; set; } 
        [JsonProperty("bank_account")]
        public string BankAccount { get; set; } 
        [JsonProperty("monocenter_makhalla_id")]
        public int MonoMfyId { get; set; }
        [JsonProperty("monocenter_address")]
        public string MonoAdress { get; set; } 
        [JsonProperty("departments_count")]
        public int DepartmentCount { get; set; } 
        [JsonProperty("total_area")]
        public string TotalArea { get; set; } 
        [JsonProperty("edu_area")]
        public string EduArea { get; set; }
        [JsonProperty("students_data")]
        public List<BandlikRequestMonoPostStudentDto> Studens { get; set; } = new();
        [JsonProperty("construction_data")]
        public BandlikRequestMonoPostItemDto Items { get; set; } = new();
        [JsonProperty("equipments_data")]
        public BandlikRequestMonoPostItem2Dto Item2s { get; set; } = new();
    }
    public class BandlikRequestMonoPostStudentDto
    {
        [JsonProperty("passport")]
        public string Passport { get; set; } 
        [JsonProperty("pin")]
        public long? Pinfl { get; set; }
        [JsonProperty("fio")]
        public string FullName { get; set; }
    }
    public class BandlikRequestMonoPostItemDto
    {
        [JsonProperty("total_cost")]
        public int TotalCost { get; set; } 
        [JsonProperty("dep_schema_url")]
        public string DepSchemaUrl { get; set; } 
        [JsonProperty("auditories_photos_url")]
        public string AuditoriesPhotoUrl { get; set; } 
        [JsonProperty("conf_doc_url")]
        public string ConfDocUrl { get; set; } 
        [JsonProperty("deed_url")]
        public string DeedUrl { get; set; }
    }
    public class BandlikRequestMonoPostItem2Dto
    {
        [JsonProperty("conf_doc_url")]
        public string ConfDocUrl { get; set; }
        [JsonProperty("equipments")]
        public List<BandlikRequestMonoPostEquipmentsDto> Equipments { get; set; } = new();
    }
    public class BandlikRequestMonoPostEquipmentsDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } 
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("total_cost")]
        public int TotalCost { get; set; }
    }
    public record BandlikPostDataDto
    {
        [JsonProperty("auth_validation")]
        public AuthValidationDto AuthValidation { get; set; } = new();

        [JsonProperty("auth")]
        public bool Auth { get; set; }
        [JsonProperty("insert")]
        public InsertDto Insert { get; set; } = new();
    }

    public class AuthValidationDto
    {
    }

    public class InsertDto
    {
        public long Id { get; set; }
    }
}
