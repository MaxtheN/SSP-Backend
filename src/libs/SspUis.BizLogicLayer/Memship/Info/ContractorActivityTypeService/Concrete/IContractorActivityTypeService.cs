using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
    public interface IContractorActivityTypeService : IStatusGeneric
    {
        PagedResult<ContractorActivityTypeListDto> GetList(SortFilterPageOptions dto);
        ContractorActivityTypeDto Get();
        ContractorActivityTypeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateContractorActivityTypeDlDto dto);
        void Update(UpdateContractorActivityTypeDlDto dto);
        void Delete(int id);
    }
}
