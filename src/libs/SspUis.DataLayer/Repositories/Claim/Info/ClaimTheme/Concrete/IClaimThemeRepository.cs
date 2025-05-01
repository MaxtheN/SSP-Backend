using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IClaimThemeRepository : IBaseEntityRepository<int, ClaimTheme, CreateClaimThemeDlDto, UpdateClaimThemeDlDto>
    {
    }
}
