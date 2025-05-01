using GenericServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusKpiRatingEmployeeDto : UpdateStatusKpiRatingEmployeeDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
}
