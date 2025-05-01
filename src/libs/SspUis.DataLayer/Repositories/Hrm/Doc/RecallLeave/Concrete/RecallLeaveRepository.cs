using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class RecallLeaveRepository
    : BaseEntityRepository<long, RecallLeave, CreateRecallLeaveDlDto, UpdateRecallLeaveDlDto, UpdateStatusRecallLeaveDlDto>
    , IRecallLeaveRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;

    public RecallLeaveRepository(
        ICrudServices crudServices,
        IAuthService authService) : base(crudServices)
    {
        _authService = authService;
        _crudServices = crudServices;
    }

    protected override void OnCreate(RecallLeave entity, CreateRecallLeaveDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllRecallLeaveCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;

        SetEntityProperties(entity);
    }

    protected override void OnUpdate(RecallLeave entity, UpdateRecallLeaveDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(entity);
    }

    private void SetEntityProperties(RecallLeave entity)
    {
        var employeeLeaveOrderTableIds = entity.Tables.Select(t => t.EmployeeLeaveOrderTableId).ToList();

        var leaveOrderTables = _crudServices.Context.Set<EmployeeLeaveOrderTable>()
            .Where(t => employeeLeaveOrderTableIds.Contains(t.Id))
            .ToDictionary(m => m.Id);

        foreach(var table in entity.Tables)
        {
            table.EmployeeId = leaveOrderTables[table.EmployeeLeaveOrderTableId].EmployeeId;
            table.DepartmentId = leaveOrderTables[table.EmployeeLeaveOrderTableId].DepartmentId;
        }
    }

    protected override IQueryable<RecallLeave> InjectFilter(IQueryable<RecallLeave> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId
            && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<RecallLeave> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables)
            .Include(x => x.Signer);
    }
}
