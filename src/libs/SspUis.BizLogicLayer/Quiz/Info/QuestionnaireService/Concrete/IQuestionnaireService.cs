using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.QuestionnaireService;

public interface IQuestionnaireService : IStatusGeneric
{
    PagedResult<QuestionnaireListDto> GetList(QuestionnaireSortFilterOptionsDto dto);
    QuestionnaireDto Get();
    HaveId<long> Create(CreateQuestionnaireDlDto dto);
    QuestionnaireDto Get(long id);
    SelectList<long> AsSelectList();
    void Update(UpdateQuestionnaireDlDto dto);
}
