using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IInstituteBillingService : IStatusGeneric
{
    PagedResult<InstituteBillingListDto> GetList(SortFilterPageOptions dto);
    InstituteBillingDto Get();
    InstituteBillingDto Get(int  id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateInstituteBillingDlDto dto);
    void Update(UpdateInstituteBillingDlDto dto);
    void Delete(int id);
}