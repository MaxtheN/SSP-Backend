using Newtonsoft.Json;

namespace SspUis.Integration.BankCredit.Models
{
    public class BankCreditReportResponseDto
    {
        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("path")]
        public object Path { get; set; }

        [JsonProperty("data")]
        public List<BankCreditReportResponse> Data { get; set; }

        [JsonProperty("response")]
        public object Response { get; set; }
    }
 
    public class BankCreditReportResponse
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("bank_mfo")]
        public string BankMfo { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }

        [JsonProperty("region_id")]
        public int? RegionId { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("district_id")]
        public int? DistrictId { get; set; }

        [JsonProperty("district_code")]
        public string DistrictCode { get; set; }

        [JsonProperty("district_name")]
        public string DistrictName { get; set; }

        [JsonProperty("application")]
        public ContractType Application { get; set; }

        [JsonProperty("contract_type_1")]
        public ContractType ContractType1 { get; set; }

        [JsonProperty("contract_type_2")]
        public ContractType ContractType2 { get; set; }

        [JsonProperty("contract_type_3")]
        public ContractType ContractType3 { get; set; }
    }
    public class ContractType
    {
        [JsonProperty("submitted_count")]
        public double SubmittedCount { get; set; }

        [JsonProperty("approved_sum")]
        public double ApprovedSum { get; set; }

        [JsonProperty("issuance_count")]
        public int IssuanceCount { get; set; }

        [JsonProperty("submitted_sum")]
        public double SubmittedSum { get; set; }

        [JsonProperty("rejected_sum")]
        public double RejectedSum { get; set; }

        [JsonProperty("canceled_sum")]
        public double CanceledSum { get; set; }

        [JsonProperty("canceled_count")]
        public int CanceledCount { get; set; }

        [JsonProperty("rejected_count")]
        public int RejectedCount { get; set; }

        [JsonProperty("issuance_sum")]
        public double IssuanceSum { get; set; }

        [JsonProperty("approved_count")]
        public int ApprovedCount { get; set; }
    }
}
