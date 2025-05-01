using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class IntegrationCertificateRequestDto
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("newVacanciesCount")]
        public int NewVacanciesCount { get; set; }

        [JsonProperty("certificateLink")]
        public string CertificateLink { get; set; }

        [JsonProperty("certificateNumber")]
        public string CertificateNumber { get; set; }

        [JsonProperty("cancelOn")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? CancelOn { get; set; }

        [JsonProperty("expireOn")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly ExpireOn { get; set; }

        [JsonProperty("docOn")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DocOn { get; set; }

        [JsonProperty("contractInfo")]
        public ContractInfo ContractInfo { get; set; } = new ContractInfo();

        [JsonProperty("contractorInfo")]
        public ContractorInfo ContractorInfo { get; set; }

        [JsonProperty("contractGraphs")]
        public List<ContractGraph> ContractGraphs { get; set; }
    }

    public class ContractGraph
    {
        [JsonProperty("yearIn")]
        public int YearIn { get; set; }

        [JsonProperty("monthIn")]
        public int MonthIn { get; set; }

        [JsonProperty("newVacanciesCount")]
        public int NewVacanciesCount { get; set; }
    }

    public class ContractInfo
    {
        [JsonProperty("contractLink")]
        public string ContractLink { get; set; }

        [JsonProperty("contractTypeId")]
        public int ContractTypeId { get; set; }

        [JsonProperty("contractType")]
        public string ContractType { get; set; }

        [JsonProperty("contractId")]
        public long ContractId { get; set; }

        [JsonProperty("contractDocOn")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly ContractDocOn { get; set; }

        [JsonProperty("contractNumber")]
        public string ContractNumber { get; set; }
    }

    public class ContractorInfo
    {
        [JsonProperty("inn")]
        public string Inn { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("regionId")]
        public int RegionId { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("districtId")]
        public int DistrictId { get; set; }

        [JsonProperty("bankName")]
        public string? BankName { get; set; }
        
        [JsonProperty("bankCode")]
        public string? BankCode { get; set; }

        [JsonProperty("oked")]
        public string Oked { get; set; }

        [JsonProperty("okedName")]
        public string OkedName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }
}


