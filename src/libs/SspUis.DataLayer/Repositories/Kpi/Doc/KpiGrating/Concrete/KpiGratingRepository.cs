using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Hrm;

using WEBASE.EF;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using SspUis.Core;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiGratingRepository
: BaseEntityRepository<long, KpiGrating, CreateKpiGratingDlDto, UpdateKpiGratingDlDto, UpdateStatusKpiGratingDlDto>, IKpiGratingRepository
{
    private readonly IAuthService _authService;

    public KpiGratingRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(KpiGrating entity, CreateKpiGratingDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<KpiGrating> InjectFilter(IQueryable<KpiGrating> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }
    protected override IQueryable<KpiGrating> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Indicators).ThenInclude(a => a.Tables);
    }
}
