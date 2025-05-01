using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class PrtnApplicationByRegionDto
    {
        public List<PrtnApplicationByRegionItemDto> Rows { get; set; }
        public Dictionary<int, string> Columns { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) ApplicationTotals { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) CertificateTotals { get; set; }
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> ApplicationColumnTotals { get; set; } = new();
        public Dictionary<int, (long? TotalCount, long? TotalNewVacanciesCount)> CertificateColumnTotals { get; set; } = new();
    }

    public class PrtnApplicationByRegionItemDto
    {
        [JsonIgnore]
        public int? PrtnContractTypeId { get; set; }
        [JsonIgnore]
        public List<int> YearIn { get; set; }
        [JsonIgnore]
        public long? CertificateCount { get; set; }
        [JsonIgnore]
        public long? NewVacanciesCount { get; set; }
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
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalCertificate { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountApplication { get; set; }
        public Dictionary<int, (long? Count, long? NewVacanciesCount)> CountCertificates { get; set; }
    }
}
