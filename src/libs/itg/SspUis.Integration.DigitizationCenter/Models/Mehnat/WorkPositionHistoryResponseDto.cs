using Newtonsoft.Json;

namespace SspUis.Integration.DigitizationCenter
{
    public class WorkPositionHistoryResponseDto
    {
        [JsonProperty("error")]
        public object? Error { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }
    public class Result
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
    public class Data
    {
        [JsonProperty("experiences")]
        public List<Experience> Experiences { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
    public class Profile
    {
        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }

        [JsonProperty("person_name")]
        public string PersonName { get; set; }

        [JsonProperty("person_patronymic")]
        public string PersonPatronymic { get; set; }

        [JsonProperty("person_surname")]
        public string PersonSurname { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }
    }
    public class Experience
    {
        [JsonProperty("action_type_id")]
        public int? ActionTypeId { get; set; }

        [JsonProperty("company_inn")]
        public string CompanyInn { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("contract_date")]
        public string ContractDate { get; set; }

        [JsonProperty("contract_number")]
        public string ContractNumber { get; set; }

        [JsonProperty("end_date")]
        public object EndDate { get; set; }

        [JsonProperty("kodp")]
        public string Kodp { get; set; }

        [JsonProperty("kodp_type")]
        public string KodpType { get; set; }

        [JsonProperty("order_date")]
        public string OrderDate { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("parent_id")]
        public object ParentId { get; set; }

        [JsonProperty("person_pin")]
        public string PersonPin { get; set; }

        [JsonProperty("position_id")]
        public int? PositionId { get; set; }

        [JsonProperty("position_name")]
        public string PositionName { get; set; }

        [JsonProperty("soato_code")]
        public string SoatoCode { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("structure_name")]
        public string StructureName { get; set; }

        [JsonProperty("transaction_id")]
        public int? TransactionId { get; set; }

        [JsonProperty("work_type")]
        public int? WorkType { get; set; }

        [JsonProperty("workplace_address")]
        public string WorkplaceAddress { get; set; }
    }
}
