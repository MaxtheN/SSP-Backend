using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class CallCenterAppealRepository :
    BaseEntityRepository<long,
        CallCenterAppeal,
        CreateCallCenterAppealDlDto,
        UpdateCallCenterAppealDlDto,
        UpdateStatusCallCenterAppealDlDto>
    , ICallCenterAppealRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICrudServices _crudServices;
    public CallCenterAppealRepository(
        ICrudServices crudServices,
        IAuthService authService,
        IUnitOfWork unitOfWork)
        : base(crudServices)
    {
        this._authService = authService;
        this._crudServices = crudServices;
        this._unitOfWork = unitOfWork;
    }

    protected override void OnCreate(CallCenterAppeal entity, CreateCallCenterAppealDlDto dto)
    {
        var org = _unitOfWork.OrganizationRepository.AllAsQueryable
            .FirstOrDefault(x =>
            x.RegionId == dto.RegionId
            && (x.OrganizationGroupId == OrganizationGroupIdConst.SSP || x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

        entity.OrganizationId = org.Id;
    }

    protected override void OnUpdate(CallCenterAppeal entity, UpdateCallCenterAppealDlDto dto)
    {
        base.OnUpdate(entity, dto);
    }

    protected override IQueryable<CallCenterAppeal> InjectFilter(IQueryable<CallCenterAppeal> query)
    {
        query = query.Where(x => x.StatusId != StatusIdConst.DELETED);
        if (_authService.IsAuthenticated)
        {
            if (_authService.Contractor != null)
                return query.Where(x => x.ContractorId == _authService.Contractor.Id);

            if (_authService.User.OrganizationId != OrganizationIdConst.SSP && _authService.User.OrganizationId != OrganizationIdConst.CALL_CENTER_ADMIN)
                query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId);
        }
        return query;
    }

    protected override IQueryable<CallCenterAppeal> ByIdQuery()
    {
        return AllAsQueryable.Include(x => x.Files);
    }
}
