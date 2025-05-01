using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Corruption;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorUnionActivityTypeRepository : BaseEntityRepository<int, ContractorUnionActivityType, CreateContractorUnionActivityTypeDlDto, UpdateContractorUnionActivityTypeDlDto>, IContractorUnionActivityTypeRepository
    {
        private readonly IAuthService _authService;

        public ContractorUnionActivityTypeRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<ContractorUnionActivityType> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
