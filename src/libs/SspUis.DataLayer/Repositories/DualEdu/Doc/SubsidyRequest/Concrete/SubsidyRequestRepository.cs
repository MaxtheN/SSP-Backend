using System.Linq;
using GenericServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SubsidyRequestRepository
    : BaseEntityRepository<long, SubsidyRequest, CreateSubsidyRequestDlDto, UpdateSubsidyRequestDlDto, UpdateStatusSubsidyRequestDlDto>,
    ISubsidyRequestRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    public SubsidyRequestRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
        : base(crudServices)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    protected override void OnCreate(SubsidyRequest entity, CreateSubsidyRequestDlDto dto)
    {
        base.OnCreate(entity, dto);
        entity.ContractorId = _authService.Contractor.Id;
        //entity.RegionId = _authService.Contractor.RegionId;
        //entity.DistrictId = _authService.Contractor.DistrictId;

        var organization = _unitOfWork.OrganizationRepository.AllAsQueryable.FirstOrDefault(x =>
            x.RegionId == _authService.Contractor.RegionId
            && (x.OrganizationGroupId == OrganizationGroupIdConst.SSP || x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));
        entity.OrganizationId = organization.Id;
    }

    protected override IQueryable<SubsidyRequest> InjectFilter(IQueryable<SubsidyRequest> query)
    {
        query = query.Where(x => x.StatusId != StatusIdConst.DELETED);

        if (_authService.Contractor != null)
            query = query.Where(x => x.ContractorId == _authService.Contractor.Id);
        else
        {
            query = query.Where(x =>
                x.StatusId != StatusIdConst.CREATED
                && x.StatusId != StatusIdConst.MODIFIED
                && x.StatusId != StatusIdConst.REVOKED);
        }

        return query;
    }
}
