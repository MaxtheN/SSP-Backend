using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class OrderToSendBusinessTripRepository
    : BaseEntityRepository<long, OrderToSendBusinessTrip, CreateOrderToSendBusinessTripDlDto, UpdateOrderToSendBusinessTripDlDto, UpdateStatusOrderToSendBusinessTripDlDto>
    , IOrderToSendBusinessTripRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;

    public OrderToSendBusinessTripRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
        _crudServices = crudServices;
    }

    protected override void OnCreate(OrderToSendBusinessTrip entity, CreateOrderToSendBusinessTripDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllOrderToSendBusinessTripCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;

        SetEntityProperties(entity);
        entity.Id2 = Guid.NewGuid();
    }
    protected override void OnUpdate(OrderToSendBusinessTrip entity, UpdateOrderToSendBusinessTripDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(entity);
    }
    private void SetEntityProperties(OrderToSendBusinessTrip entity)
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
    protected override IQueryable<OrderToSendBusinessTrip> InjectFilter(IQueryable<OrderToSendBusinessTrip> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<OrderToSendBusinessTrip> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables)
            .Include(x => x.Signer);
    }
}
