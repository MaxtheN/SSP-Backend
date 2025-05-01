using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class MassPlannedCalculationRepository
    : BaseEntityRepository<long, MassPlannedCalculation, CreateMassPlannedCalculationDlDto, UpdateMassPlannedCalculationDlDto, UpdateStatusMassPlannedCalculationDlDto>
    , IMassPlannedCalculationRepository
{
    private readonly IAuthService _authService;

    public MassPlannedCalculationRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    protected override void OnCreate(MassPlannedCalculation entity, CreateMassPlannedCalculationDlDto dto)
    {
        //if (_authService.HasPermission(ModuleCode.AllAppointEmployeeCreate))
        //{
        //    entity.OrganizationId = dto.OrganizationId.Value;
        //}
        //else
            entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<MassPlannedCalculation> InjectFilter(IQueryable<MassPlannedCalculation> query)
    {
        query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        return query;
    }

    protected override IQueryable<MassPlannedCalculation> ByIdQuery()
    {
        return AllAsQueryable;
    }
}
