using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationListDto : ILinkToEntity<JoinAntiCorruptionApplication>
    {
        public ApplicationListDto Application { get; set; }
        public long Id { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }
        public long? CertificateId { get; set; }
        public int? CertificateStatusId { get; set; }
        public int CorruptionReviewTypeId { get; set; }
        public string CorruptionReviewType { get; set; }
        public int ContractorActivityTypeId { get; set; }
        public string ContractorActivityType { get; set; }
        public int ContractorUnionActivityTypeId { get; set; }
        public string ContractorUnionActivityType { get; set; }
        public int UnionMemberCount { get; set; }
        public int CurrentStepId { get; set; }
        public string CurrentStep { get; set; }
        public int AvgEmployeesCount { get; set; }
        public decimal PrevYearlyEarnings { get; set; }
    }
}
