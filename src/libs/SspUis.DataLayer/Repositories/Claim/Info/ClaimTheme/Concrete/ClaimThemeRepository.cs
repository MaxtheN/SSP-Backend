using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimThemeRepository : BaseEntityRepository<int, ClaimTheme, CreateClaimThemeDlDto, UpdateClaimThemeDlDto>, IClaimThemeRepository
    {
        private readonly IAuthService _authService;

        public ClaimThemeRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<ClaimTheme> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
