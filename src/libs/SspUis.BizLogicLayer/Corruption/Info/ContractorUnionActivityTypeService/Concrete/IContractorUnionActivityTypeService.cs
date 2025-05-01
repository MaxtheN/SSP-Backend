using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public interface IContractorUnionActivityTypeService : IStatusGeneric
    {
        PagedResult<ContractorUnionActivityTypeListDto> GetList(SortFilterPageOptions dto);
        ContractorUnionActivityTypeDto Get();
        ContractorUnionActivityTypeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateContractorUnionActivityTypeDlDto dto);
        void Update(UpdateContractorUnionActivityTypeDlDto dto);
        void Delete(int id);
    }
}
