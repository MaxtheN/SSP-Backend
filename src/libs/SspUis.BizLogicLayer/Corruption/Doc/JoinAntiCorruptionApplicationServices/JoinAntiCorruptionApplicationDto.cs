using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    [PrintableModel("Korrupsiya arizasi.", TableIdConst.CORRUPTION__DOC_JOIN_ANTI_CORRUPTION_APPLICATION)]
    public class JoinAntiCorruptionApplicationDto : UpdateJoinAntiCorruptionApplicationDlDto,
        ILinkToEntity<JoinAntiCorruptionApplication>,
        IBaseApplication<ApplicationDto>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public new ApplicationDto Application { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public string CorruptionReviewType { get; set; }
        public int? CorruptionReviewTypeId { get; set; }
        public string ContractorActivityType { get; set; }
        //public int ContractorActivityTypeId { get; set; }
        public string ContractorUnionActivityType { get; set; }
        public int? ContractorUnionActivityTypeId { get; set; }
        public int? UnionMemberCount { get; set; }
        public int? AvgEmployeesCount { get; set; }
        public decimal PrevYearlyEarnings { get; set; }
        public int? CurrencyId { get; set; }
        public List<JoinAntiCorruptionApplicationFileDlDto> Files { get; set; } = new();
        public List<JoinAntiCorruptionApplicationEmployeeDlDto> Employees { get; set; } = new();
        public List<JoinAntiCorruptionApplicationParticipateDlDto> Participates { get; set; } = new();
        public List<JoinAntiCorruptionApplicationTableDlDto> Tables { get; set; } = new();
        #region Actions
        public bool CanAccept { get; set; }
        public bool CanAcceptSSP { get; set; }
        public bool CanAcceptOmbudsman { get; set; }
        public bool CanReject { get; set; }
        public bool CanEdit { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }
        #endregion
    }
}
