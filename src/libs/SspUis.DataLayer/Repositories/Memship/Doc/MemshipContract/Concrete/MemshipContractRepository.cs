using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Memship;

public class MemshipContractRepository
    : BaseEntityRepository<long, MemshipContract, CreateMemshipContractDlDto, UpdateMemshipContractDlDto, UpdateStatusMemshipContractDlDto>
    , IMemshipContractRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudService;

    public MemshipContractRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
        _crudService = crudServices;
    }

    protected override void OnCreate(MemshipContract entity, CreateMemshipContractDlDto dto)
    {
        //entity.OrganizationId = OrganizationIdConst.SSP;
        dto.IsRead = false;
        SetEntityProperties(dto, entity);
    }

    protected override void OnUpdate(MemshipContract entity, UpdateMemshipContractDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(dto, entity);
    }

    private void SetEntityProperties<TDto>(MemshipContractDlDto<TDto> dto, MemshipContract entity)
          where TDto : MemshipContractDlDto<TDto>
    {
        var ma = _crudService.Context.Set<Application>()
            .Include(x => x.MemshipApplication)
            .FirstOrDefault(m => m.Id == entity.ApplicationId);

        entity.ContractorId = ma.ContractorId.Value;
        entity.RegionId = ma.MemshipApplication.ChooseLocation ? ma.MemshipApplication.ChoosedRegionId.Value : ma.RegionId;
        entity.DistrictId = ma.MemshipApplication.ChooseLocation ? ma.MemshipApplication.ChoosedDistrictId.Value : ma.DistrictId;
    }

    protected override IQueryable<MemshipContract> InjectFilter(IQueryable<MemshipContract> query)
    {
        query = query.Include(m => m.Application).ThenInclude(m => m.MemshipApplication);

        query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

        if (_authService.Contractor != null)
            return query.Where(a => a.ContractorId == _authService.Contractor.Id);

        if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
            return query = query.Where(a =>
                                a.MemshipContractTypeId == MemshipContractTypeIdConst.FREE &&
                                a.StatusId != StatusIdConst.SENT_FOR_REVIEW &&
                                a.RegionId == _authService.Organization.RegionId);


        return query.Where(a =>
            a.OrganizationId == _authService.Organization.Id
            || _authService.Organization.Id == OrganizationIdConst.SSP);
    }
    //protected override IQueryable<MemshipContract> InjectFilter(IQueryable<MemshipContract> query)
    //{
    //    query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

    //    if (_authService.Contractor != null)
    //    {
    //        query = query.Where(a => a.ContractorId == _authService.Contractor.Id);
    //        if (query.Any(a => a.ApplicationId != null))
    //        {
    //            query = query.Include(m => m.Application).ThenInclude(m => m.MemshipApplication);
    //        }
    //    }
    //    else if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
    //    {
    //        if (query.Any(a => a.ApplicationId != null))
    //        {
    //            query = query.Include(m => m.Application).ThenInclude(m => m.MemshipApplication);
    //        }
    //        query = query.Where(a =>
    //            a.MemshipContractTypeId == MemshipContractTypeIdConst.FREE &&
    //            a.StatusId != StatusIdConst.SENT_FOR_REVIEW &&
    //            a.RegionId == _authService.Organization.RegionId);
    //    }
    //    else
    //    {
    //        query = query.Where(a =>
    //            a.OrganizationId == _authService.Organization.Id ||
    //            _authService.Organization.Id == OrganizationIdConst.SSP);
    //        if (query.Any(a => a.ApplicationId != null))
    //        {
    //            query = query.Include(m => m.Application).ThenInclude(m => m.MemshipApplication);
    //        }
    //    }

    //    return query;
    //}
    protected override IQueryable<MemshipContract> ByIdQuery()
    {
        return AllAsQueryable;
    }

    public (bool hasDocument, long documentId, long applicationId) IsThereMembershipContract(string inn)
    {
        var result = _crudService.Context.Set<MemshipContract>().Include(a => a.Application)
                .Select(mc => new
                {
                    mc.ApplicationId,
                    mc.DocOn,
                    ContractorInn = mc.Contractor.Inn,
                    mc.StatusId,
                    mc.Id
                })
                .OrderByDescending(mc => mc.DocOn)
                .FirstOrDefault(mc => mc.ContractorInn == inn &&
                                      mc.StatusId == StatusIdConst.SIGNED);

        if (result == null)
            return (false, 0, 0);

        return (true, result.Id, result.ApplicationId.Value);
    }
    public (bool hasDocument, long documentId) IsThereMembershipContractForBank(string inn)
    {
        var result = _crudService.Context.Set<MemshipContract>()
                .Select(mc => new
                {
                    mc.DocOn,
                    ContractorInn = mc.Contractor.Inn,
                    mc.StatusId,
                    mc.Id
                })
                .OrderByDescending(mc => mc.DocOn)
                .FirstOrDefault(mc => mc.ContractorInn == inn &&
                                      mc.StatusId == StatusIdConst.SIGNED);

        if (result == null)
            return (false, 0);

        return (true, result.Id);
    }
}
