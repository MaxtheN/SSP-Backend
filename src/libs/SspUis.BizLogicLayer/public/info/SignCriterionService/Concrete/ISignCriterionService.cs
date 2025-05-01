using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService;

public interface ISignCriterionService : IStatusGeneric
{
    PagedResult<SignCriterionListDto> GetList(SortFilterPageOptions dto);
    SignCriterionDto Get();
    SignCriterionDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateSignCriterionDlDto dto);
    void Update(UpdateSignCriterionDlDto dto);
    void Delete(int id);
}
