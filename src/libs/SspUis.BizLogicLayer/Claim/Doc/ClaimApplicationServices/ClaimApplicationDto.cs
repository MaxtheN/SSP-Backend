using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    [PrintableModel("Davo arizasi.", TableIdConst.CLAIM__DOC_CLAIM_APPLICATION)]
    public class ClaimApplicationDto : UpdateClaimApplicationDlDto, ILinkToEntity<ClaimApplication>,
        IBaseApplication<SspUis.BizLogicLayer.ApplicationDto>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public new SspUis.BizLogicLayer.ApplicationDto Application { get; set; }
        public string Details { get; set; }
        public string ClaimApplicationType { get; set; }
        public string Currency { get; set; }
        public string ClaimTheme { get; set; }
        public string MemshipContractDocNumber { get; set; }
        public DateOnly? MemshipContractDocOn { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? MainDebt { get; set; }
        public decimal? CalculedPenalty { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? Percent { get; set; }
        public long? EmployeeManageId { get; set; }
        public string? Message { get; set; }
        public List<ClaimApplicationFileDlDto> Files { get; set; } = new();
        public List<ClaimApplicationTableDlDto> Tables { get; set; } = new();

        public bool CanEdit { get; set; }
        public bool CanSend { get; set; }
        public bool CanRevoke { get; set; }
        //public bool CanReject { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        public bool CanCreateMediationPlan { get; set; }
        public bool CanEmployeeAttechment { get; set; }
        public bool CanCreateApplicationForCourt { get; set; }
        public bool CanCreateWhithOutMediation { get; set; }
    }
}
