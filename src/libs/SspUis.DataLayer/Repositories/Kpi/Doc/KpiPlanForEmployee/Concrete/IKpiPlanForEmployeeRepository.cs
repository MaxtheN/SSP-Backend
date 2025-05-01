using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Kpi;

public interface IKpiPlanForEmployeeRepository : IBaseEntityRepository<long, KpiPlanForEmployee, CreateKpiPlanForEmployeeDlDto, UpdateKpiPlanForEmployeeDlDto, UpdateStatusKpiPlanForEmployeeDlDto>
{
}
