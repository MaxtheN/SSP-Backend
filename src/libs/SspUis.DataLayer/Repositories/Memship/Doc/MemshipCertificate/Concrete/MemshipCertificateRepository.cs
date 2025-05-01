using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class MemshipCertificateRepository
    : BaseEntityRepository<long, MemshipCertificate, CreateMemshipCertificateDlDto, UpdateMemshipCertificateDlDto, UpdateStatusMemshipCertificateDlDto>
    , IMemshipCertificateRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudService;

    public MemshipCertificateRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
        _crudService = crudServices;
    }

    protected override void OnCreate(MemshipCertificate entity, CreateMemshipCertificateDlDto dto)
    {
        var contract = _crudService.Context.Set<MemshipContract>().FirstOrDefault(x => x.Id == dto.MemshipContractId);
        entity.RegionId = contract.RegionId;
        entity.DistrictId = contract.DistrictId;
        entity.IsRead = false;
        if (_authService is null || _authService.User is null)
            entity.OrganizationId = 1;
        else
            entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<MemshipCertificate> InjectFilter(IQueryable<MemshipCertificate> query)
    {
        query = query.Where(x => /*x.OrganizationId == _authService.User.OrganizationId &&*/ x.StatusId != StatusIdConst.DELETED);

        if (_authService.Contractor != null)
            return query.Where(a => a.ContractorId == _authService.Contractor.Id);

        //if (_authService.HasPermission(ModuleCode.MemshipCertificateViewAll) || _authService.UserName == "certificate")
        //    return query;

        if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
        {
            query = query.Where(a => a.RegionId == _authService.Organization.RegionId);
        }

        return query;//.Where(a => a.OrganizationId == _authService.Organization.Id || _authService.Organization.Id == OrganizationIdConst.SSP);

    }

    protected override IQueryable<MemshipCertificate> ByIdQuery()
    {
        return AllAsQueryable.Include(c => c.Files);
    }
}