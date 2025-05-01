using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.Integration.TadbirkorFund
{
    public class TadbirkorFundContractorDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("dateon")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DateOn { get; set; }

        [JsonProperty("tin_pinfl")]
        public string TinPinfl { get; set; } = null!;

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("businesssectorid")]
        public int BusinessSectorId { get; set; }

        [JsonProperty("businesssctorname")]
        public string BusinessSectorName { get; set; } = null!;

        [JsonProperty("businesssectortypeid")]
        public int? BusinessSectortypeid { get; set; }

        [JsonProperty("businesssectortypename")]
        public string? BusinessSectorTypeName { get; set; }

        [JsonProperty("financialassistanceid")]
        public int FinancialAssistanceId { get; set; }

        [JsonProperty("financialassistancename")]
        public string FinancialAsistanceName { get; set; } = null!;

        [JsonProperty("bankcode")]
        public string BankCode { get; set; } = null!;

        [JsonProperty("bankname")]
        public string BankName { get; set; } = null!;

        [JsonProperty("regionsoato")]
        public string RegionSoato { get; set; } = null!;

        [JsonProperty("regionname")]
        public string RegionName { get; set; } = null!;

        [JsonProperty("districtsoato")]
        public string DistrictSoato { get; set; } = null!;

        [JsonProperty("districtname")]
        public string DistrictName { get; set; } = null!;

        [JsonProperty("aidamount")]
        public decimal AidAmount { get; set; }

        [JsonProperty("newjobposition")]
        public string NewJobPosition { get; set; } = null!;

        [JsonProperty("realjobposition")]
        public string RealJobPosition { get; set; } = null!;

        [JsonProperty("credits")]
        public List<Credit> Credits { get; set; } = new List<Credit>();
    }

    public class Credit
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currencyid")]
        public int CurrencyId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = null!;
    }

}