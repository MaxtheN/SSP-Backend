using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices
{
    public interface IContractorActivityGroupService : IStatusGeneric
    {
        PagedResult<ContractorActivityGroupListDto> GetList(SortFilterPageOptions dto);
        ContractorActivityGroupDto Get();
        ContractorActivityGroupDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateContractorActivityGroupDlDto dto);
        void Update(UpdateContractorActivityGroupDlDto dto);
        void Delete(int id);
    }
}
