using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Claim;

public class MediationPlanRepository
    : BaseEntityRepository<long, MediationPlan, CreateMediationPlanDlDto, UpdateMediationPlanDlDto, UpdateStatusMediationPlanDlDto>
    , IMediationPlanRepository
{
    private readonly IAuthService _authService;

    public MediationPlanRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(MediationPlan entity, CreateMediationPlanDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<MediationPlan> InjectFilter(IQueryable<MediationPlan> query)
    {
        if (_authService.Contractor != null)
            query = query.Where(p => p.ContractorId == _authService.Contractor.Id);
        else
            query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<MediationPlan> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Application)
            .Include(x => x.PreviousMediationPlan)
            .Include(x => x.Contractor);
    }
}