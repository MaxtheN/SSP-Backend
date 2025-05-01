using SspUis.BizLogicLayer.RoleServices;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AccessServices
{
    public interface IAccessService : IStatusGeneric
    {
        PagedResult<AccessListDto> GetList(SortFilterPageOptions dto);
        AccessDto Get(int id);
        SelectList<int> AsSelectList();
        void Update(UpdateAccessibilityDlDto dto);        
    }
}