using Newtonsoft.Json;
using SspUis.Core;
using System.Runtime.CompilerServices;

namespace SspUis.Integration.MarkaziyBank
{
    public class MarkaziyBankContractorCreditHistoryResponseDto
    {
        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("data")]
        public List<MarkaziyBankContractorCreditHistoryDto> Data { get; set; } = new List<MarkaziyBankContractorCreditHistoryDto>();

        [JsonProperty("response")]
        public string? Response { get; set; }
    }

    public class MarkaziyBankContractorCreditHistoryDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fromdate")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]

        public DateOnly FromDate { get; set; }

        [JsonProperty("todate")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]

        public DateOnly ToDate { get; set; }

        [JsonProperty("tin")]
        public int Tin { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bankcode")]
        public string Bankcode { get; set; }

        [JsonProperty("bankname")]
        public string Bankname { get; set; }

        [JsonProperty("creditamount")]
        public int Creditamount { get; set; }

        [JsonProperty("accountnumbername")]
        public string AccountNumberName { get; set; }

        [JsonProperty("accountnumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("numberofaccount")]
        public string NumberOfAccount { get; set; }

        [JsonProperty("credittype")]
        public string CreditType { get; set; }

        [JsonProperty("credittypeid")]
        public int CreditTypeId { get; set; }

        [JsonProperty("loaninterestrate")]
        public int Loaninterestrate { get; set; }

        [JsonProperty("numberofcreditsreceived")]
        public int NumberOfCreditsReceived { get; set; }
    }
}