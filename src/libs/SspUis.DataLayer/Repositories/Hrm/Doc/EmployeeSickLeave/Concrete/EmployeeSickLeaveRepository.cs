using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSickLeaveRepository
    : BaseEntityRepository<long, EmployeeSickLeave, CreateEmployeeSickLeaveDlDto, UpdateEmployeeSickLeaveDlDto, UpdateStatusEmployeeSickLeaveDlDto>
    , IEmployeeSickLeaveRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;

    public EmployeeSickLeaveRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
        _crudServices = crudServices;
    }

    protected override void OnCreate(EmployeeSickLeave entity, CreateEmployeeSickLeaveDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllEmployeeSickLeaveCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;

        SetEntityProperties(entity);
    }
    protected override void OnUpdate(EmployeeSickLeave entity, UpdateEmployeeSickLeaveDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(entity);
    }
    private void SetEntityProperties(EmployeeSickLeave entity)
    {
        var employeeManageIds = entity.Tables.Select(t => t.EmployeeManageId).ToList();

        var manages = _crudServices.Context.Set<EmployeeManage>()
            .Include(x => x.Table)
            .Where(m => employeeManageIds.Contains(m.Id))
            .ToDictionary(m => m.Id);

        foreach (var table in entity.Tables)
        {
            table.EmployeeId = manages[(long)table.EmployeeManageId].EmployeeId;
            table.DepartmentId = manages[(long)table.EmployeeManageId].DepartmentId;
        }
    }
    protected override IQueryable<EmployeeSickLeave> InjectFilter(IQueryable<EmployeeSickLeave> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<EmployeeSickLeave> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables);
    }
}
