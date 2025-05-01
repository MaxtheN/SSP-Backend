using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IQuestionnaireRepository : IBaseEntityRepository<long, Questionnaire, CreateQuestionnaireDlDto, UpdateQuestionnaireDlDto>
{
}
