using Newtonsoft.Json;

namespace SspUis.Integration.IntegrationCertificate
{
    public record IntegrationCertificateResponseDto
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("success")]
        public string? Success { get; set; }
        [JsonProperty("reason")]
        public string? Reason { get; set; }
    }
}
