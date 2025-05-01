using GenericServices;
using SspUis.Core;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusEmployeeLeaveOrderDto : UpdateStatusEmployeeLeaveOrderDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        [LocalizedRequired]
        public string SignedData { get; set; }
    }
    public class SignStatusEmployeeLeaveOrdeDto : UpdateStatusEmployeeLeaveOrderDto
    {
        public SignStatusEmployeeLeaveOrdeDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
    }
}
