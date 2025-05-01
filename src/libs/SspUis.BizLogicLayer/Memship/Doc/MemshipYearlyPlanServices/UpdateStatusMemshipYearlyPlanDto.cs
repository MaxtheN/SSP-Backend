using GenericServices;
using SspUis.DataLayer.Repositories.Memship;

namespace SspUis.BizLogicLayer.Memship
{
    public class UpdateStatusMemshipYearlyPlanDto : UpdateStatusMemshipYearlyPlanDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
