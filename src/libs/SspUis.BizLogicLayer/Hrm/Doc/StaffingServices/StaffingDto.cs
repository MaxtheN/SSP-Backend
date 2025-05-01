using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingDto : UpdateStaffingDlDto, ILinkToEntity<Staffing>, IDocument
    {
        public string Status { get; set; } = null!;
        public string? StaffingTemplateName { get; set; } = null;
        public string? OrgSettlementAccountCode { get; set; } = null;
        public string? SettlementAccountSource { get; set; } = null;
        public string? FunctionalItemOfExpense { get; set; } = null;
        public string? SchoolGroupContingent { get; set; } = null;
        public int TableId { get; set; } = TableIdConst.HRM__DOC_STAFFING;
        public int StatusId { get; set; }
        new public List<StaffingPositionDto> Positions { get; set; } = new();
        new public List<StaffingIndicatorValueDto> IndicatorValues { get; set; } = new();

        #region Actions
        public bool CanModify { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }
        #endregion
    }
}
