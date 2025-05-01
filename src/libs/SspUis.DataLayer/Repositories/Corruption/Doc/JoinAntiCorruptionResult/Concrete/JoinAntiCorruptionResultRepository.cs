using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Corruption;

public class JoinAntiCorruptionResultRepository :
    BaseEntityRepository<long, JoinAntiCorruptionResult, CreateJoinAntiCorruptionResultDlDto, UpdateJoinAntiCorruptionResultDlDto, UpdateStatusJoinAntiCorruptionResultDlDto>
    , IJoinAntiCorruptionResultRepository
{
    private readonly IAuthService _authService;
    public JoinAntiCorruptionResultRepository(
        ICrudServices crudServices,
        IAuthService authService
        ) : base(crudServices)
    {
        this._authService = authService;
    }

    protected override void OnCreate(JoinAntiCorruptionResult entity, CreateJoinAntiCorruptionResultDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;
    }

    protected override IQueryable<JoinAntiCorruptionResult> InjectFilter(IQueryable<JoinAntiCorruptionResult> query)
        => query.Where(x => x.StatusId != StatusIdConst.DELETED);

    protected override IQueryable<JoinAntiCorruptionResult> ByIdQuery()
    {
        return AllAsQueryable
            .Include(x => x.Tables);
    }
}
