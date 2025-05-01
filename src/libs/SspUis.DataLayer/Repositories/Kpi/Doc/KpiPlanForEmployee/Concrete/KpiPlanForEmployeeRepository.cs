using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Kpi;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiPlanForEmployeeRepository 
    : BaseEntityRepository<long, KpiPlanForEmployee, CreateKpiPlanForEmployeeDlDto, UpdateKpiPlanForEmployeeDlDto, UpdateStatusKpiPlanForEmployeeDlDto>, IKpiPlanForEmployeeRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;
    private readonly IUnitOfWork _unitOfWork;

    public KpiPlanForEmployeeRepository(
        ICrudServices crudServices,
        IUnitOfWork unitOfWork,
        IAuthService authService) : base(crudServices)


    {
        _authService = authService;
        _crudServices = crudServices;
        _unitOfWork = unitOfWork;

    }
    protected override void OnCreate(KpiPlanForEmployee entity, CreateKpiPlanForEmployeeDlDto dto)
    {
        entity.OrganizationId = dto.OrganizationId;
    }
    protected override IQueryable<KpiPlanForEmployee> InjectFilter(IQueryable<KpiPlanForEmployee> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }
    protected override IQueryable<KpiPlanForEmployee> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Tables).ThenInclude(s => s.EmployeeManage).Include(a => a.Tables).ThenInclude(a => a.Creates).ThenInclude(s => s.Indicator);
    }
}
