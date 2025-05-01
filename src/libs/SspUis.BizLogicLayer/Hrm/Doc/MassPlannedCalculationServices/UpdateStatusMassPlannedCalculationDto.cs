using GenericServices;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusMassPlannedCalculationDto : UpdateStatusMassPlannedCalculationDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
