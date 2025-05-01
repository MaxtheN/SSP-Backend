using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeLeaveOrderRepository :
    BaseEntityRepository<long, EmployeeLeaveOrder, CreateEmployeeLeaveOrderDlDto, UpdateEmployeeLeaveOrderDlDto, UpdateStatusEmployeeLeaveOrderDlDto>
    , IEmployeeLeaveOrderRepository
{
    private readonly IAuthService _authService;
    public EmployeeLeaveOrderRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        this._authService = authService;
    }

    protected override void OnCreate(EmployeeLeaveOrder entity, CreateEmployeeLeaveOrderDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllEmployeeLeaveOrderCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;
        entity.Id2 = Guid.NewGuid();
    }

    protected override IQueryable<EmployeeLeaveOrder> InjectFilter(IQueryable<EmployeeLeaveOrder> query)
    {
        query = query.Where(x => x.StatusId != StatusIdConst.DELETED);
        if (_authService.User != null)
            return query.Where(x => x.OrganizationId == _authService.User.OrganizationId);
        else
            return query;
    }

    protected override IQueryable<EmployeeLeaveOrder> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables)
            .Include(x => x.Signer);
    }

    protected override IQueryable<EmployeeLeaveOrder> ByIdQuery(bool applyFilter)
    {
        return AllAsQueryable
            .Include(x => x.Tables)
            .Include(x => x.Signer);
    }
}
