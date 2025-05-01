using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimOrganizationTypeRepository : BaseEntityRepository<int, ClaimOrganizationType, CreateClaimOrganizationTypeDlDto, UpdateClaimOrganizationTypeDlDto>, IClaimOrganizationTypeRepository
    {
        private readonly IAuthService _authService;

        public ClaimOrganizationTypeRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<ClaimOrganizationType> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
