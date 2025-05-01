using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IQuestionRepository : IBaseEntityRepository<int, Question, CreateQuestionDlDto, UpdateQuestionDlDto>
{ }
