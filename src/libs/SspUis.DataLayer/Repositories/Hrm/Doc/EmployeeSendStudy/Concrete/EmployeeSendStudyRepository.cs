using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSendStudyRepository
    : BaseEntityRepository<long, EmployeeSendStudy, CreateEmployeeSendStudyDlDto, UpdateEmployeeSendStudyDlDto, UpdateStatusEmployeeSendStudyDlDto>
    , IEmployeeSendStudyRepository
{
    private readonly IAuthService _authService;

    public EmployeeSendStudyRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(EmployeeSendStudy entity, CreateEmployeeSendStudyDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllEmployeeSendStudyCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;

        entity.Id2 = Guid.NewGuid();
    }

    protected override IQueryable<EmployeeSendStudy> InjectFilter(IQueryable<EmployeeSendStudy> query)
    {
        query = query.Where(x => x.StatusId != StatusIdConst.DELETED);
        if (_authService.User != null)
            return query.Where(x => x.OrganizationId == _authService.User.OrganizationId);
        else
            return query;
    }

    protected override IQueryable<EmployeeSendStudy> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables)
            .Include(x => x.Signer);
    }
}
