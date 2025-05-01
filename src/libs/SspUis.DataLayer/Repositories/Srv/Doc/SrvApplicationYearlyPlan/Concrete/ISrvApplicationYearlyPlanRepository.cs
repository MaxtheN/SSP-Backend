using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface ISrvApplicationYearlyPlanRepository : IBaseEntityRepository<long, SrvApplicationYearlyPlan, CreateSrvApplicationYearlyPlanDlDto, UpdateSrvApplicationYearlyPlanDlDto,UpdateStatusSrvApplicationYearlyPlanDlDto>
{
}
