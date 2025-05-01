using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorContactRepository : BaseEntityRepository<long, ContractorContact, CreateContractorContactDlDto, UpdateContractorContactDlDto>, IContractorContactRepository
    {
        private readonly IAuthService _authService;
        public ContractorContactRepository(ICrudServices crudServices, IAuthService authService) : base(crudServices)
        {
            _authService = authService;
        }
    }
}
