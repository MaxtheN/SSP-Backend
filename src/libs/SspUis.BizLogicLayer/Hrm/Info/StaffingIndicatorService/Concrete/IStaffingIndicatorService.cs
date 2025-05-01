using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices
{
    public interface IStaffingIndicatorService : IStatusGeneric
    {
        PagedResult<StaffingIndicatorListDto> GetList(SortFilterPageOptions dto);
        StaffingIndicatorDto Get();
        StaffingIndicatorDto Get(int id);
        SelectList<int> AsSelectList(bool filterByOrganizationalStructure = true);
        HaveId<int> Create(CreateStaffingIndicatorDlDto dto);
        void Update(UpdateStaffingIndicatorDlDto dto);
        void Delete(int id);
    }
}
