using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IMediationRepository : IBaseEntityRepository<long, Mediation, CreateMediationDlDto, UpdateMediationDlDto, UpdateStatusMediationDlDto>
    {

    }
}
