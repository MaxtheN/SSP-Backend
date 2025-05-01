using GenericServices;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

    public class UpdateStatusEmployeeSickLeaveDto : UpdateStatusEmployeeSickLeaveDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }

