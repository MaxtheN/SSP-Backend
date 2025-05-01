using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusTimesheetDto : UpdateStatusTimesheetDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
