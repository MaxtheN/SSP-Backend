using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingListDto : DocumentListDto<long>, ILinkToEntity<Staffing>, IHaveIdProp<long>, IHaveStatusId
    {
        public string DocNumber { get; set; } = null!;
        public decimal DocSum { get; set; }
        public string? Details { get; set; } = null!;
        public int FinanceYear { get; set; }
        public string OrgSettlementAccount { get; set; } = null;
        public string Organization { get; set; } = null;
        public string SettlementAccountSource { get; set; } = null;
        public string OrganizationalClassification { get; set; } = null;
        public long? OrgSettlementAccountId { get; set; } = null;
        public int? SettlementAccountSourceId { get; set; } = null;
        public int? OrganizationalClassificationId { get; set; } = null;
        public string Status { get; set; } = null!;
        public string StaffingType { get; set; } = null!;
        public int? OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }

        #region Actions
        public bool CanReject { get; set; }
        public bool CanReceived { get; set; }
        public bool CanClone{ get; set; }
        public bool CanSend { get; set; }
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        public bool CanArchive { get; set; }
        public bool CanRecallArchive { get; set; }
        #endregion
    }
}
