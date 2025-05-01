using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SrvApplicationYearlyPlanRepository
    : BaseEntityRepository<long, SrvApplicationYearlyPlan, CreateSrvApplicationYearlyPlanDlDto, UpdateSrvApplicationYearlyPlanDlDto, UpdateStatusSrvApplicationYearlyPlanDlDto>
    , ISrvApplicationYearlyPlanRepository
{
    private readonly IAuthService _authService;

    public SrvApplicationYearlyPlanRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(SrvApplicationYearlyPlan entity, CreateSrvApplicationYearlyPlanDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<SrvApplicationYearlyPlan> InjectFilter(IQueryable<SrvApplicationYearlyPlan> query)
    {
        if (_authService.User.OrganizationId == OrganizationGroupIdConst.SSP)
        {
            return query;
        }
        else
        {
            query = query.Where(x => x.RegionId == _authService.Organization.RegionId && x.StatusId != StatusIdConst.DELETED);
            return query;
        }
    }

    protected override IQueryable<SrvApplicationYearlyPlan> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Tables).Include(a => a.Files);
    }
}
