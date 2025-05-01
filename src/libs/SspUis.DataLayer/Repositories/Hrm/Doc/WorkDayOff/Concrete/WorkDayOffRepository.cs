using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class WorkDayOffRepository :
    BaseEntityRepository<long, WorkDayOff, CreateWorkDayOffDlDto, UpdateWorkDayOffDlDto, UpdateStatusWorkDayOffDlDto>
    , IWorkDayOffRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;
    public WorkDayOffRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        this._authService = authService;
        this._crudServices = crudServices;
    }

    protected override void OnCreate(WorkDayOff entity, CreateWorkDayOffDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
        SetEntityProperties(entity);

    }

    protected override void OnUpdate(WorkDayOff entity, UpdateWorkDayOffDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(entity);
    }
    private void SetEntityProperties(WorkDayOff entity)
    {
        var employeeManageIds = entity.Tables.Select(t => t.EmployeeManageId).ToList();

        var manages = _crudServices.Context.Set<EmployeeManage>()
            .Include(x => x.Table)
            .Where(m => employeeManageIds.Contains(m.Id))
            .ToDictionary(m => m.Id);

        foreach(var table in entity.Tables)
        {
            table.EmployeeId = manages[(long)table.EmployeeManageId].EmployeeId;
            table.DepartmentId = manages[(long)table.EmployeeManageId].DepartmentId;
        }
    }

    protected override IQueryable<WorkDayOff> InjectFilter(IQueryable<WorkDayOff> query)
    {
        query = query.Where(x => x.StatusId != StatusIdConst.DELETED);
        if (_authService.User != null)
            return query.Where(x => x.OrganizationId == _authService.User.OrganizationId);
        else
            return query;
    }

    protected override IQueryable<WorkDayOff> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables)
            .Include(x => x.Signer);
    }
}
