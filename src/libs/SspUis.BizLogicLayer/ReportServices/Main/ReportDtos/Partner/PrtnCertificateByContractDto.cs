using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class PrtnCertificateByContractDto
    {
        public List<PrtnCertificateByContractItemDto> Rows { get; set; }
        public Dictionary<int, string> Columns { get; set; }
        //public Dictionary<int, string> ColumnBanks { get; set; }
        public (long TotalCount, long TotalNewVacanciesCount) CertificateTotalsWithVacancies { get; set; }     // Umumiy sertifikatlar jamlanmasi passdagi column
        //public (long TotalCount, long TotalSum) CertificateTotals { get; set; }     // Umumiy sertifikatlar jamlanmasi passdagi column
        public Dictionary<int, (long TotalCount, long TotalNewVacanciesCount)> CertificateColumnTotals { get; set; } = new();    //har bir column uchun jamlanma 
        //public Dictionary<int, (long TotalCount, long TotalSum)> CertificateColumnBankTotals { get; set; } = new();    //har bir bank uchun jamlanma passdagi columnlar
    }

    public class PrtnCertificateByContractItemDto
    {
        [JsonIgnore]
        public int PrtnContractTypeId { get; set; }
        public int? BankId { get; set; }

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
        [JsonIgnore]
        public long CertificateCount { get; set; }
        [JsonIgnore]
        public long CertificateSum { get; set; }
        [JsonIgnore]
        public long SumOfTotal { get; set; }
        /*[JsonIgnore]
        public long NewVacanciesCount { get; set; }*/
        [JsonIgnore]
        public long CertificateNewVacanciesCount { get; set; }
        //public (long TotalCount, long TotalSum) TotalCertificate { get; set; }    // umumiy berilgan sertifikat row uchun
        public (long TotalCount, long TotalNewVacanciesCount) TotalCertificateWithVacancies { get; set; }    // umumiy berilgan sertifikat row uchun
        //public Dictionary<long, (long Count, long SumOfTotal)> CountCertificates { get; set; }   //Har bir shartnoma turi uchun
        public Dictionary<int, (long Count, long NewVacanciesCount)> CountCertificatesWithVacancies { get; set; }  
    }
}