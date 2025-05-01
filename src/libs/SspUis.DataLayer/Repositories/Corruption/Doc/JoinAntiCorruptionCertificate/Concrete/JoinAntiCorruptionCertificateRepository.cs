using System;
using System.Linq;
using GenericServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Corruption;

public class JoinAntiCorruptionCertificateRepository :
    BaseEntityRepository<long, JoinAntiCorruptionCertificate, CreateJoinAntiCorruptionCertificateDlDto, UpdateJoinAntiCorruptionCertificateDlDto>
    , IJoinAntiCorruptionCertificateRepository
{
    private readonly IAuthService _authService;
    public JoinAntiCorruptionCertificateRepository(
        ICrudServices crudServices,
        IAuthService authService
        ) : base(crudServices)
    {
        this._authService = authService;
    }

    protected override void OnCreate(JoinAntiCorruptionCertificate entity, CreateJoinAntiCorruptionCertificateDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
        entity.Id2 = Guid.NewGuid();
    }

    protected override IQueryable<JoinAntiCorruptionCertificate> InjectFilter(IQueryable<JoinAntiCorruptionCertificate> query)
    {
        if (_authService.Contractor != null)
            return query.Where(x => x.ContractorId == _authService.Contractor.Id);
        else
            return query.Where(x => x.StatusId != StatusIdConst.DELETED);
    }
}
