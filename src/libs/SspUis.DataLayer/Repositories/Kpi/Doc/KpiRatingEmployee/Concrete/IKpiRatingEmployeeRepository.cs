using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IKpiRatingEmployeeRepository : IBaseEntityRepository<long, KpiRatingEmployee, CreateKpiRatingEmployeeDlDto, UpdateKpiRatingEmployeeDlDto,UpdateStatusKpiRatingEmployeeDlDto>
{
}
