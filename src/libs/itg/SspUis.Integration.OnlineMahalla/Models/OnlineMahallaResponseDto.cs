using Newtonsoft.Json;

namespace SspUis.Integration.OnlineMahalla
{
    public record OnlineMahallaResponseDto<T>
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
    public record OnlineMahallaDataDto
    {
        [JsonProperty("district_id")]
        public long MfyId { get; set; }
        [JsonProperty("district_name")]
        public string MfyNameRu { get; set; } = null!;
        [JsonProperty("district_uz")]
        public string MfyNameUz { get; set; } = null!;
        [JsonProperty("obl_soato")]
        public long RegionSoatoCode { get; set; }
        [JsonProperty("area_soato")]
        public long DistrictSoatoCode { get; set; }
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }
        [JsonProperty("obl_name")]
        public string RegionName { get; set; } = null!;
        [JsonProperty("area_name")]
        public string DistrictName { get; set; } = null!;
        [JsonProperty("state")]
        public int State { get; set; }
    }

    public record OnlineMahallaPostDataDto
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
