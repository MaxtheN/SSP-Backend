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
    public class ContractorSettlementAccountRepository : BaseEntityRepository<long, ContractorSettlementAccount, CreateContractorSettlementAccountDlDto, UpdateContractorSettlementAccountDlDtoo>, IContractorSettlementAccountRepository
    {
        private readonly IAuthService _aAuthService;

        public ContractorSettlementAccountRepository(ICrudServices crudServices, 
            IAuthService aAuthService) : base(crudServices)
        {
            _aAuthService = aAuthService;
        }
    }
}
