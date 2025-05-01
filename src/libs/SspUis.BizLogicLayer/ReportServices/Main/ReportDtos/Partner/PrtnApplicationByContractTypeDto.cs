using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class PrtnApplicationByContractTypeDto
    {
        public List<PrtnApplicationByContractTypeItemDto> Rows { get; set; }
        public Dictionary<int, string> Columns { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) ApplicationTotals { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) ContractTotals { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) ContractCreatedTotals { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) ContractSignedTotals { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) CertificateTotals { get; set; }
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> ApplicationColumnTotals { get; set; } = new();
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> ContractColumnTotals { get; set; } = new();
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> ContractColumnThatCreatedTotals { get; set; } = new();
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> ContractColumnThatSignedTotals { get; set; } = new();
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> CertificateColumnTotals { get; set; } = new();
    }

    public class PrtnApplicationByContractTypeItemDto
    {
        [JsonIgnore]
        public int? PrtnContractTypeId { get; set; }
        [JsonIgnore]
        public long? ContractCount { get; set; }
        [JsonIgnore]
        public long? CreatedContractCount { get; set; }
        [JsonIgnore]
        public long? SignedContractCount { get; set; }
        [JsonIgnore]
        public long? CertificateCount { get; set; }
        [JsonIgnore]
        public long? NewVacanciesCount { get; set; }
        [JsonIgnore]
        public long? ContractNewVacanciesCount { get; set; }
        [JsonIgnore]
        public long? CreatedContractNewVacanciesCount { get; set; }
        [JsonIgnore]
        public long? PassExpertiseApplicationCount { get; set; }
        [JsonIgnore]
        public long? PassExpertiseApplicationNewVacanciesCount  { get; set; }
        [JsonIgnore]
        public long? SignedContractNewVacanciesCount { get; set; }
        [JsonIgnore]
        public long? CertificateNewVacanciesCount { get; set; }
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }

        public long? MfyId { get; set; }
        public string Mfy { get; set; }

        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalApplication { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalContract { get; set; }
        public (long? TotalPassExCount, long? TotalPassExNewVacanciesCount) TotalPassExContract { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalContractCreated { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalContractSigned { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalCertificate { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountApplication { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountContracts { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountContractsCreated { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountContractsSigned { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountCertificates { get; set; }
    }
}