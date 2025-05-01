using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSendTrainRepository
    : BaseEntityRepository<long, EmployeeSendTrain, CreateEmployeeSendTrainDlDto, UpdateEmployeeSendTrainDlDto, UpdateStatusEmployeeSendTrainDlDto>
    , IEmployeeSendTrainRepository
{
    private readonly IAuthService _authService;

    public EmployeeSendTrainRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(EmployeeSendTrain entity, CreateEmployeeSendTrainDlDto dto)
    {
        if (_authService.HasPermission(ModuleCode.AllEmployeeSendTrainCreate))
        {
            entity.OrganizationId = dto.OrganizationId.Value;
        }
        else
            entity.OrganizationId = _authService.User.OrganizationId;

        entity.Id2 = Guid.NewGuid();
    }

    protected override IQueryable<EmployeeSendTrain> InjectFilter(IQueryable<EmployeeSendTrain> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<EmployeeSendTrain> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables).ThenInclude(x => x.Employee).ThenInclude(x => x.Person)
            .Include(x => x.Signer);
    }
}
