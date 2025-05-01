using GenericServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusSrvYearlyPlanDto : UpdateStatusSrvYearlyPlanDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
