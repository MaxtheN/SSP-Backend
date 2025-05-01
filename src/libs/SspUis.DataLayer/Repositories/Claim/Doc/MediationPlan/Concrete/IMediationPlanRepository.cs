using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Claim;

public interface IMediationPlanRepository : IBaseEntityRepository<long, MediationPlan, CreateMediationPlanDlDto, UpdateMediationPlanDlDto,UpdateStatusMediationPlanDlDto>
{
}
