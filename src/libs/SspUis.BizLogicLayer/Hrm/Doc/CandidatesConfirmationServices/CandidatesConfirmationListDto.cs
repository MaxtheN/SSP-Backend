using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationListDto : ILinkToEntity<CandidatesConfirmation>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public DateOnly DocOn { get; set; }
        public string DocNumber { get; set; }
        public string DocContent { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public int PositionId { get; set; }
        public string Position { get; set; }
        public string GeneralConclusion { get; set; }
        public DateTime CreatedAt { get; set; }
        #region Actions
        public bool CanDelete { get; set; }
        public bool CanSend { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        #endregion
    }
}
