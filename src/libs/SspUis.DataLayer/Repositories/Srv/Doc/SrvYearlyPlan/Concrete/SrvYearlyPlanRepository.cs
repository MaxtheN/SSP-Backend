using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SrvYearlyPlanRepository
    : BaseEntityRepository<long, SrvYearlyPlan, CreateSrvYearlyPlanDlDto, UpdateSrvYearlyPlanDlDto, UpdateStatusSrvYearlyPlanDlDto>
    , ISrvYearlyPlanRepository
{
    private readonly IAuthService _authService;

    public SrvYearlyPlanRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(SrvYearlyPlan entity, CreateSrvYearlyPlanDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<SrvYearlyPlan> InjectFilter(IQueryable<SrvYearlyPlan> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<SrvYearlyPlan> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Regions).ThenInclude(a => a.Districts);
    }
}
