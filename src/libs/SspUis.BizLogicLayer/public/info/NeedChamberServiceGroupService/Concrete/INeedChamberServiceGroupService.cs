using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface INeedChamberServiceGroupService : IStatusGeneric
    {
        PagedResult<NeedChamberServiceGroupListDto> GetList(SortFilterPageOptions dto);
        NeedChamberServiceGroupDto Get();
        NeedChamberServiceGroupDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateNeedChamberServiceGroupDlDto dto);
        void Update(UpdateNeedChamberServiceGroupDlDto dto);
        void Delete(int id);
    }
}
