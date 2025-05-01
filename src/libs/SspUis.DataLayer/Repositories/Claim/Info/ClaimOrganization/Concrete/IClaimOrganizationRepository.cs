using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IClaimOrganizationRepository : IBaseEntityRepository<int, ClaimOrganization, CreateClaimOrganizationDlDto, UpdateClaimOrganizationDlDto>
    {
    }
}
