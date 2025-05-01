using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;
public class QuestionnaireRepository : BaseEntityRepository<long, Questionnaire, CreateQuestionnaireDlDto, UpdateQuestionnaireDlDto>, IQuestionnaireRepository
{
    public QuestionnaireRepository(ICrudServices crudServices)
    : base(crudServices)
    { }

    protected override IQueryable<Questionnaire> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Translates)
                             .Include(a => a.QuestionnaireGroups)
                                .ThenInclude(a => a.QuestionnaireQuestions)
                                    .ThenInclude(a => a.QuestionnaireAnswers);
    }

    protected override IQueryable<Questionnaire> InjectFilter(IQueryable<Questionnaire> query)
    {
        return query.Where(a => a.StateId == StateIdConst.ACTIVE);
    }
}

