using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorActivityTypeRepository : BaseEntityRepository<int, ContractorActivityType, CreateContractorActivityTypeDlDto, UpdateContractorActivityTypeDlDto>, IContractorActivityTypeRepository
    {
        private readonly IAuthService _authService;

        public ContractorActivityTypeRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<ContractorActivityType> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
