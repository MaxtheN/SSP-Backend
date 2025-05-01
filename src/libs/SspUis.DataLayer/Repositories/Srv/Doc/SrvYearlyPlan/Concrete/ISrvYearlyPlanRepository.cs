using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface ISrvYearlyPlanRepository : IBaseEntityRepository<long, SrvYearlyPlan, CreateSrvYearlyPlanDlDto, UpdateSrvYearlyPlanDlDto,UpdateStatusSrvYearlyPlanDlDto>
{
}
