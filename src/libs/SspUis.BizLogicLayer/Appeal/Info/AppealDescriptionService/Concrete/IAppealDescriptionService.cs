using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public interface IAppealDescriptionService : IStatusGeneric
{
    PagedResult<AppealDescriptionListDto> GetList(SortFilterPageOptions dto);
    AppealDescriptionDto Get();
    AppealDescriptionDto Get(int id);
    SelectList<int> AsSelectList(bool hasParent);
    HaveId<int> Create(CreateAppealDescriptionDlDto dto);
    void Update(UpdateAppealDescriptionDlDto dto);
    void Delete(int id);
}
