using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Quiz.Enum;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Quiz;

public class QuizManualService : StatusGenericHandler, IQuizManualService
{
    private readonly IUnitOfWork _unitOfWork;

    public QuizManualService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public SelectList<int> AnswerTypeSelectList()
    {
        return _unitOfWork.Context.Set<AnswerType>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> InspectionTypeSelectList()
    {
        return _unitOfWork.Context.Set<InspectionType>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> QuestionnaireTypeSelectList()
    {
        return _unitOfWork.Context.Set<QuestionnaireType>().Include(a => a.Translates).AsSelectList();
    }
}
