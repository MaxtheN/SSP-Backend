using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public interface IAppealTypeArriveService : IStatusGeneric
{
    PagedResult<AppealTypeArriveListDto> GetList(SortFilterPageOptions dto);
    AppealTypeArriveDto Get();
    AppealTypeArriveDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateAppealTypeArriveDlDto dto);
    void Update(UpdateAppealTypeArriveDlDto dto);
    void Delete(int id);
}
