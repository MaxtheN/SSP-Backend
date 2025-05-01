using GenericServices;
using SspUis.Core;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusRecallLeaveDto : UpdateStatusRecallLeaveDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        [LocalizedRequired]
        public string SignedData { get; set; }
    }
    public class SignStatusRecallLeaveDto : UpdateStatusRecallLeaveDto
    {
        public SignStatusRecallLeaveDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
    }
}
