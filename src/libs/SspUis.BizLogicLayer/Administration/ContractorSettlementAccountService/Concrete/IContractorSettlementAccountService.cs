using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.Administration.ContractorContactService;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Administration.ContractorSettlementAccountService
{
    public interface IContractorSettlementAccountService : IStatusGeneric
    {
        List<ContractorSettlementAccountListDto> GetList();
        public ContractorSettlementAccountDto Get(long id);
        public ContractorSettlementAccountDto Get();
        void Update(UpdateContractorSettlementAccountDlDtoo dto);
        HaveId<int> Create(CreateContractorSettlementAccountDlDto dto);
        bool SetMain(long id);
        void Delete(int id);
    }
}
