using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IQuestionGroupRepository : IBaseEntityRepository<int, QuestionGroup, CreateQuestionGroupDlDto, UpdateQuestionGroupDlDto>
{ }
