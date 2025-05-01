using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class PlannedCalculationRepository
    : BaseEntityRepository<long, PlannedCalculation, CreatePlannedCalculationDlDto, UpdatePlannedCalculationDlDto, UpdateStatusPlannedCalculationDlDto>
    , IPlannedCalculationRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;

    public PlannedCalculationRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
        _crudServices = crudServices;
    }

    protected override void OnCreate(PlannedCalculation entity, CreatePlannedCalculationDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllPlannedCalculationCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;

        SetEntityProperties(entity);
    }
    protected override void OnUpdate(PlannedCalculation entity, UpdatePlannedCalculationDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(entity);
    }
    private void SetEntityProperties(PlannedCalculation entity)
    {
        var employeeManageIds = entity.Tables.Select(t => t.EmployeeManageId).ToList();

        var manages = _crudServices.Context.Set<EmployeeManage>()
            .Include(x => x.Table)
            .Where(m => employeeManageIds.Contains(m.Id))
            .ToDictionary(m => m.Id);

        foreach (var table in entity.Tables)
        {
            table.EmployeeId = manages[(long)table.EmployeeManageId].EmployeeId;
            entity.DepartmentId = manages[(long)table.EmployeeManageId].DepartmentId;
            table.PositionId = manages[(long)table.EmployeeManageId].PositionId;
        }
    }
    protected override IQueryable<PlannedCalculation> InjectFilter(IQueryable<PlannedCalculation> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<PlannedCalculation> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables);
    }
}
