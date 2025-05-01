using GenericServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusSrvApplicationYearlyPlanDto : UpdateStatusSrvApplicationYearlyPlanDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
