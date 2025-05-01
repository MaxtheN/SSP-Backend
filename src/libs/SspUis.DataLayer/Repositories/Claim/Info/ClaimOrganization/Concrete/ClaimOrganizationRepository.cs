using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimOrganizationRepository : BaseEntityRepository<int, ClaimOrganization, CreateClaimOrganizationDlDto, UpdateClaimOrganizationDlDto>, IClaimOrganizationRepository
    {
        private readonly IAuthService _authService;

        public ClaimOrganizationRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<ClaimOrganization> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
