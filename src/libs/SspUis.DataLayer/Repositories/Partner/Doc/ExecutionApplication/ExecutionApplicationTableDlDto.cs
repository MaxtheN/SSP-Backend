using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using System.Text.Json.Serialization;

namespace SspUis.DataLayer.Repositories
{
    public class ExecutionApplicationTableDlDto : EntityDto<ExecutionApplicationTableDlDto, ExecutionApplicationTable>,
        IHaveIdProp<long>,
        ILinkToEntity<ExecutionApplicationTable>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long PrtnCertificateId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PrtnNewVacanciesCount { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int ProjectNewVacanciesCount { get; set; }
        [LocalizedRequired]
        public decimal AverageSalary { get; set; }
        [LocalizedRequired]
        public decimal Salary { get; set; }
    }
}
