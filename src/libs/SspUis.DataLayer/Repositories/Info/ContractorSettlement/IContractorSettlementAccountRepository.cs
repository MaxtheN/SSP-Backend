using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IContractorSettlementAccountRepository : IBaseEntityRepository<long, ContractorSettlementAccount, CreateContractorSettlementAccountDlDto, UpdateContractorSettlementAccountDlDtoo>
    {
    }
}
