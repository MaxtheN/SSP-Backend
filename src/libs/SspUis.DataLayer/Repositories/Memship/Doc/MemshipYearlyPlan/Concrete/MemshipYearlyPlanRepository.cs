using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Memship;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Memship;

public class MemshipYearlyPlanRepository
    : BaseEntityRepository<long, MemshipYearlyPlan, CreateMemshipYearlyPlanDlDto, UpdateMemshipYearlyPlanDlDto, UpdateStatusMemshipYearlyPlanDlDto>
    , IMemshipYearlyPlanRepository
{
    private readonly IAuthService _authService;


    public MemshipYearlyPlanRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(MemshipYearlyPlan entity, CreateMemshipYearlyPlanDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<MemshipYearlyPlan> InjectFilter(IQueryable<MemshipYearlyPlan> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<MemshipYearlyPlan> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Tables);
    }
}
