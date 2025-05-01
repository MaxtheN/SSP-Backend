using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Appeal;

public class AppealApplicationRepository :
    BaseEntityRepository<long,
        AppealApplication,
        CreateAppealApplicationDlDto,
        UpdateAppealApplicationDlDto,
        UpdateStatusAppealApplicationDlDto>
    , IAppealApplicationRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICrudServices _crudServices;
    public AppealApplicationRepository(
        ICrudServices crudServices,
        IAuthService authService,
        IUnitOfWork unitOfWork)
        : base(crudServices)
    {
        this._authService = authService;
        this._crudServices = crudServices;
        this._unitOfWork = unitOfWork;
    }

    protected override void OnCreate(AppealApplication entity, CreateAppealApplicationDlDto dto)
    {

        var org = _unitOfWork.OrganizationRepository.AllAsQueryable
            .FirstOrDefault(x =>
            x.RegionId == dto.RegionId
            && (x.OrganizationGroupId == OrganizationGroupIdConst.SSP || x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

        entity.OrganizationId = org.Id;
    }

    protected override void OnUpdate(AppealApplication entity, UpdateAppealApplicationDlDto dto)
    {
        base.OnUpdate(entity, dto);
    }

    protected override IQueryable<AppealApplication> InjectFilter(IQueryable<AppealApplication> query)
    {
        query = query.Where(x => x.StatusId != StatusIdConst.DELETED);

        if (_authService.IsAuthenticated)
        {
            query = query.Where(x => !x.IsCreatedByChamber);

            if (_authService.Contractor != null)
                return query.Where(x => x.ContractorId == _authService.Contractor.Id);

            query = query.Where(x =>
             x.AppealFormatTypeId == AppealFormatTypeIdConst.Electronic
             ? x.StatusId != StatusIdConst.CREATED
                 || x.StatusId != StatusIdConst.MODIFIED
             : true);

            if (_authService.Organization != null && _authService.Organization.Id != OrganizationIdConst.SSP)
                query = query.Where(x => x.OrganizationId == _authService.Organization.Id);
        }
        return query;
    }

    protected override IQueryable<AppealApplication> ByIdQuery()
    {
        return AllAsQueryable.Include(x => x.Files);
    }
}
