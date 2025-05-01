using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Quiz;

public interface IQuizManualService : IStatusGeneric
{
    SelectList<int> AnswerTypeSelectList();
    SelectList<int> InspectionTypeSelectList();
    SelectList<int> QuestionnaireTypeSelectList();
}
