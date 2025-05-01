using System.Text.Json.Serialization;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class PrtnApplicationAndContractInfoDto
    {
        public string PrtnContractType { get; set; }
        public int PrtnContractTypeId { get; set; }
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }

        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorPhoneNumber { get; set; }

        public long TotalPrtnApplicationSentCount { get; set; }
        public long TotalPrtnApplicationSentForReviewCount { get; set; }
        public long TotalPrtnApplicationPassExpertisesCount { get; set; }
        public long TotalPrtnApplicationSentForExpertisesCount { get; set; }
        public long TotalPrtnApplicationSentForExpertisesCount2 { get; set; }
        public long TotalPrtnApplicationNotPassExpertisesCount { get; set; }
        public long TotalPrtnApplicationNotPassExpertisesCount2 { get; set; }
        public long TotalPrtnApplicationSignedCount { get; set; }
        public long TotalPrtnApplicationSignningCount { get; set; }
        public long TotalPrtnApplicationSentAcceptedCount { get; set; }
        public long TotalPrtnApplicationSentRejectedCount { get; set; }

        public long TotalPrtnApplicationCanceledWhithOutContractCount { get; set; }
        public long TotalPrtnApplicationCanceledWhithOutRejectCount { get; set; }

        public long TotalPrtnApplicationSentRevokedCount { get; set; }
        public long TotalPrtnApplicationCanceledCount { get; set; }
        public long TotalPrtnContractCount { get; set; }
        public long TotalPrtnContractCancelCount { get; set; }
        public long TotalPrtnContractRejectedCount { get; set; }

        public long TotalPrtnContractCanceledWhithOutCertificateCount { get; set; }
        public long TotalPrtnContractRejectWhithOutCertificateCount { get; set; }

        public long TotalPrtnCertificateCanceledApplicationCount { get; set; }
        public long TotalPrtnCertificateRejectApplicationCount { get; set; }

        public long TotalPrtnCertificateCount { get; set; }
        public long TotalPrtnCertificateCanceledCount { get; set; }

        public long TotalPrtnCertificateRejectedCount { get; set; }
        public long TotalNewVacanciesCount { get; set; }
        public long TotalPrtnApplicationCount { get; set; }
        public long TotalPrtnApplicationIsOffersCount { get; set; }
        [JsonIgnore]
        public long? NewVacanciesCount { get; set; }
        public (long? TotalCount, long? TotalNewVacanciesCount) TotalApplication { get; set; }


        public long IsBeingCosideredApplicationCount { get; set; }
        public int ExpiredIsBeingCosideredApplicationCount { get; set; }

        public long SendToExpertiseCount { get; set; }
        public int ExpiredSendToExpertiseCount { get; set; }
        public int NotPassCount { get; set; }
        public int ExpiredResentToExpiredCount { get; set; }
        public long PassCount1 { get; set; }
        public long SignExpireOnCount1 { get; set; }
        public long PassCount2 { get; set; }
        public long SigningExpireOnCount { get; set; }
        public long SigningCount { get; set; }
        public long SignExpireOnCount2 { get; set; }
        public long NotGeneratedCertificatesCount { get; set; }
        public long GeneratedCertificatesCount { get; set; }
    }
}
