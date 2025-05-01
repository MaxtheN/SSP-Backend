using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public interface IPrtnContractTypeService : IStatusGeneric
    {
        PagedResult<PrtnContractTypeListDto> GetList(SortFilterPageOptions dto);
        PrtnContractTypeDto Get();
        PrtnContractTypeDto Get(int id);
        List<PrtnContractTypeTableDto> GetTables(int id);
        SelectList<int> GetTablesBySignOrganizationType(TablesBySignOrganizationTypeDtoFilter filter);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreatePrtnContractTypeDlDto dto);
        void Update(UpdatePrtnContractTypeDlDto dto);
        void Delete(int id);
    }
}
