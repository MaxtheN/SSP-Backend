using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class ChastisementRepository :
    BaseEntityRepository<long, Chastisement, CreateChastisementDlDto, UpdateChastisementDlDto, UpdateStatusChastisementDlDto>
    , IChastisementRepository
{
    private readonly IAuthService _authService;
    public ChastisementRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        this._authService = authService;
    }

    protected override void OnCreate(Chastisement entity, CreateChastisementDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllChastisementCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
        {
            entity.OrganizationId = _authService.User.OrganizationId;
        }
        entity.Id2 = Guid.NewGuid();
        foreach (var item in entity.Tables)
        {
            var employeeManage = Context.Set<EmployeeManage>().FirstOrDefault(x => x.Id == item.EmployeeManageId);
            item.PositionId = employeeManage.PositionId;
            item.EmployeeId = employeeManage.EmployeeId;
            item.DepartmentId = employeeManage.DepartmentId;
            item.EmployeeRate = employeeManage.EmploymentRate.Value;
        }
    }

    protected override IQueryable<Chastisement> InjectFilter(IQueryable<Chastisement> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId
            && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<Chastisement> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables).ThenInclude(b => b.Employee).ThenInclude(a => a.Person)
            .Include(x => x.Signer)
            .Include(x => x.Files);
    }
}
