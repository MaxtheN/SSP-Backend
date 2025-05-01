using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class KpiRatingEmployeeRepository
    : BaseEntityRepository<long, KpiRatingEmployee, CreateKpiRatingEmployeeDlDto, UpdateKpiRatingEmployeeDlDto, UpdateStatusKpiRatingEmployeeDlDto>
    , IKpiRatingEmployeeRepository
{
    private readonly IAuthService _authService;

    public KpiRatingEmployeeRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(KpiRatingEmployee entity, CreateKpiRatingEmployeeDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<KpiRatingEmployee> InjectFilter(IQueryable<KpiRatingEmployee> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<KpiRatingEmployee> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Tables).ThenInclude(a => a.Points);
    }
}
