using System.Collections.Generic;

namespace SspUis.DataLayer.Repositories
{
    public interface IJoinAntiCorruptionApplicationDlDto
    {
        public long ApplicationId { get; set; }
        public int CorruptionReviewTypeId { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public int ContractorActivityTypeId { get; set; }
        public int ContractorUnionActivityTypeId { get; set; }
        public int UnionMemberCount { get; set; }
        public int AvgEmployeesCount { get; set; }
        public decimal PrevYearlyEarnings { get; set; }
        public int CurrencyId { get; set; }

        public List<JoinAntiCorruptionApplicationFileDlDto> Files { get; set; }
        public List<JoinAntiCorruptionApplicationEmployeeDlDto> Employees { get; set; }
        public List<JoinAntiCorruptionApplicationParticipateDlDto> Participates { get; set; }
        public List<JoinAntiCorruptionApplicationTableDlDto> Tables { get; set; }
    }
}
