using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class QuestionRepository : BaseEntityRepository<int, Question, CreateQuestionDlDto, UpdateQuestionDlDto>, IQuestionRepository
{
    public QuestionRepository(ICrudServices crudServices)
        : base(crudServices)
    { }

    protected override IQueryable<Question> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Translates);
    }

    protected override IQueryable<Question> InjectFilter(IQueryable<Question> query)
    {
        return query.Where(a => a.StateId == StateIdConst.ACTIVE);
    }
}
