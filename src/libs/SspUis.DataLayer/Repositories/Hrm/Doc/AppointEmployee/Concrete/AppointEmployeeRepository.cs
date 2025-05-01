using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class AppointEmployeeRepository :
    BaseEntityRepository<long, AppointEmployee, CreateAppointEmployeeDlDto, UpdateAppointEmployeeDlDto, UpdateStatusAppointEmployeeDlDto>
    , IAppointEmployeeRepository
{
    private readonly IAuthService _authService;
    public AppointEmployeeRepository(
        ICrudServices crudServices,
        IAuthService authService
        ) : base(crudServices)
    {
        this._authService = authService;
    }
    protected override void OnCreate(AppointEmployee entity, CreateAppointEmployeeDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllAppointEmployeeCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;
        SetEntityProperties(entity, dto);
        entity.Id2 = Guid.NewGuid();
    }

    protected override void OnUpdate(AppointEmployee entity, UpdateAppointEmployeeDlDto dto)
    {
        SetEntityProperties(entity, dto);
    }
    private void SetEntityProperties<TDto>(AppointEmployee entity, AppointEmployeeDlDto<TDto> dto)
           where TDto : AppointEmployeeDlDto<TDto>
    {
        foreach (var table in dto.Tables.Where(a => a.EmployeeManageId == 0))
        {
            table.EmployeeManageId = null;
        }
    }
    protected override IQueryable<AppointEmployee> InjectFilter(IQueryable<AppointEmployee> query)
    {
        if (_authService.HasPermission(ModuleCode.AppointEmployeeSignerView) && !_authService.User.IsAdmin)
            query = query.Where(a => a.Signer.Any(b => b.EmployeeManageId == _authService.User.EmployeeManageId));

        if (_authService.User.IsAdmin)
        {
            return query.Where(a => a.StatusId != StatusIdConst.DELETED);
        }
        else if (_authService.User != null)
        {
            return query = query.Where(x => (x.OrganizationId == _authService.User.OrganizationId 
                || (_authService.User.OrganizationId == OrganizationIdConst.SSP && x.Signer.Any(x => x.EmployeeManageId == _authService.User.EmployeeManageId))
                && x.StatusId != StatusIdConst.DELETED));
        }
        else
            return query;
    }

    protected override IQueryable<AppointEmployee> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables).ThenInclude(x => x.Employee).ThenInclude(x => x.Person)
            .Include(x => x.Signer);
    }
}
