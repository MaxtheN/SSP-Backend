using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Administration.ContractorContactService
{
    public interface IContractorContactService : IStatusGeneric
    {
        List<ContractorContactListDto> GetList();
        public ContractorContactDto Get(long id);
        public ContractorContactDto Get();
        void Update(UpdateContractorContactDlDto dto);
        HaveId<int> Create(CreateContractorContactDlDto dto);
        void Delete(int id);
    }
}
