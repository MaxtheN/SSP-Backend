using Newtonsoft.Json;

namespace SspUis.Integration.DavAktiv
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DavAktivContractorDto
    {
        [JsonProperty("id_document")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("send_date")]
        public string SendDate { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("ns10")]
        public string Ns10 { get; set; } = string.Empty;

        [JsonProperty("ns11")]
        public string Ns11 { get; set; } = string.Empty;

        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;

        [JsonProperty("tin")]
        public string Tin { get; set; } = string.Empty;

        [JsonProperty("share1")]
        public string Share1 { get; set; } = string.Empty;

        [JsonProperty("org_type_name")]
        public string Orgtype { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
    }


     public class DavAktivContractorDtoDesc
    {
        [JsonProperty("id_document")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("SEND_DATE")]
        public string SendDate { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("ns10")]
        public string Ns10 { get; set; } = string.Empty;

        [JsonProperty("ns11")]
        public string Ns11 { get; set; } = string.Empty;

        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;

        [JsonProperty("tin")]
        public string Tin { get; set; } = string.Empty;

        [JsonProperty("share1")]
        public string Share1 { get; set; } = string.Empty;

        [JsonProperty("org_type_name")]
        public string Orgtype { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class DavAktivContractorResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public List<DavAktivContractorDto> Data { get; set; } = null!;

		[JsonProperty("description")]
		public DavAktivContractorDtoDesc Description { get; set; } = null!;
	}


}