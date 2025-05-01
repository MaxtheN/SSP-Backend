using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Memship;

public interface IMemshipYearlyPlanRepository : IBaseEntityRepository<long, MemshipYearlyPlan, CreateMemshipYearlyPlanDlDto, UpdateMemshipYearlyPlanDlDto,UpdateStatusMemshipYearlyPlanDlDto>
{
}
