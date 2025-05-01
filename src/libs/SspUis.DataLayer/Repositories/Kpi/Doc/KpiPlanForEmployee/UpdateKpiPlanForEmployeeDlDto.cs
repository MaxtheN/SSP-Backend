

using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Kpi;

public class UpdateKpiPlanForEmployeeDlDto: KpiPlanForEmployeeDlDto<UpdateKpiPlanForEmployeeDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
