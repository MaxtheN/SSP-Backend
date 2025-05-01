using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface ISpecialtyBillingService : IStatusGeneric
{
    PagedResult<SpecialtyBillingListDto> GetList(SortFilterPageOptions dto);
    SpecialtyBillingDto Get();
    SpecialtyBillingDto Get(int id);
    SelectList<int> AsSelectList(int? instituteId = null);
    HaveId<int> Create(CreateSpecialtyBillingDlDto dto);
    void Update(UpdateSpecialtyBillingDlDto dto);
    void Delete(int id);
}