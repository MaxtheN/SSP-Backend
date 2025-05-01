using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationCourtApplicationRepository
    : BaseApplicationRepository
        <ArbitrationCourtApplication,
        CreateArbitrationCourtApplicationDlDto,
        UpdateArbitrationCourtApplicationDlDto,
        UpdateStatusArbitrationCourtApplicationDlDto,
        UpdateStepArbitrationCourtApplicationDlDto>,
    IArbitrationCourtApplicationRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    public ArbitrationCourtApplicationRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
        : base(crudServices, authService, unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    protected override void OnCreate(ArbitrationCourtApplication entity, CreateArbitrationCourtApplicationDlDto dto)
    {
        SetEntityProperties(entity, dto);
    }
    protected void SetEntityProperties<TDto>(ArbitrationCourtApplication entity, TDto dto)
        where TDto : ArbitrationCourtApplicationDlDto<TDto>
    {
        if (dto.ContractorId != null)
        {
            var contractor = _unitOfWork.Context.Set<EfClasses.Contractor>().Include(x => x.BusinessmanUserInContractors).FirstOrDefault(x => x.Id == dto.ContractorId);
            var region = _unitOfWork.Context.Set<Region>().FirstOrDefault(x => x.Id == contractor.RegionId);
            var district = _unitOfWork.Context.Set<District>().FirstOrDefault(x => x.Id == contractor.DistrictId);

            entity.Application.DistrictId = district.Id;
            entity.Application.RegionId = region.Id;
            entity.Application.DistrictName = district.FullName;
            entity.Application.RegionName = region.FullName;
        }
        else if (dto.IsForeignContractor)
        {

            var forForeingregion = _unitOfWork.Context.Set<Region>().FirstOrDefault(x => x.Id == RegionIdConst.TASHKENT);
            var forForeingdistrict = _unitOfWork.Context.Set<District>().FirstOrDefault(x => x.Id == DistrictIdConst.MIRZOULUGBEK);

            entity.Application.DistrictId = forForeingdistrict.Id;
            entity.Application.RegionId = forForeingregion.Id;
            entity.Application.DistrictName = forForeingdistrict.FullName;
            entity.Application.RegionName = forForeingregion.FullName;
        }
        if (_authService.Contractor == null)
            entity.IsCreatedByErp = true;
        else
            entity.IsCreatedByErp = false;

    }
    protected override IQueryable<ArbitrationCourtApplication> InjectFilter(IQueryable<ArbitrationCourtApplication> query)
    {
        query = base.InjectFilter(query);
        if (_authService.Contractor != null)
            return query.Where(x => x.Application.ContractorId == _authService.Contractor.Id);
        if (_unitOfWork.ArbitrationJudgeRepository.AllAsQueryable.Any(x => x.PersonId == _authService.User.PersonId))
        {
            var data = query.Where(doc => doc.Signer.Any(s => s.ArbitrationJudge.PersonId == _authService.User.PersonId));
            return data;
        }
    


        //if (_authService.User.OrganizationId == OrganizationIdConst.SSP)
        //    return query.Where(x => x.IsForeignContractor ? true : x.OrganizationId == _authService.User.OrganizationId);
        //else
        //    return query.Where(x => x.OrganizationId == _authService.Organization.Id);

        query = query.Where(x =>
            ((_authService.User.OrganizationId == OrganizationIdConst.COURT_OF_ARBITRATION
                    || _authService.User.OrganizationId == OrganizationIdConst.SSP))
                ? true
                : x.OrganizationId == _authService.User.OrganizationId);

        return query;
    }

    protected override IQueryable<ArbitrationCourtApplication> ByIdQuery()
    {
        return AllAsQueryable
            .Include(m => m.Signer)
            .Include(m => m.Application)
            .Include(m => m.Application.Contractor)
            .Include(m => m.Application.Region)
            .Include(m => m.Application.District)
            ;
    }
}
