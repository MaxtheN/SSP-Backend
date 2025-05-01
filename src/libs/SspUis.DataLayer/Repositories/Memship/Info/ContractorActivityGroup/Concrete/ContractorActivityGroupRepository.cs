using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorActivityGroupRepository : BaseEntityRepository<int, ContractorActivityGroup, CreateContractorActivityGroupDlDto, UpdateContractorActivityGroupDlDto>, IContractorActivityGroupRepository
    {
        private readonly IAuthService _authService;

        public ContractorActivityGroupRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<ContractorActivityGroup> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
