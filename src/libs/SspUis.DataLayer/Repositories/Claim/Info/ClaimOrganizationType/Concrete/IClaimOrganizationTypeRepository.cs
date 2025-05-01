using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IClaimOrganizationTypeRepository : IBaseEntityRepository<int, ClaimOrganizationType, CreateClaimOrganizationTypeDlDto, UpdateClaimOrganizationTypeDlDto>
    {
    }
}
