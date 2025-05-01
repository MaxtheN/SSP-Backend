using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class QuestionGroupRepository : BaseEntityRepository<int, QuestionGroup, CreateQuestionGroupDlDto, UpdateQuestionGroupDlDto>, IQuestionGroupRepository
{
    public QuestionGroupRepository(ICrudServices crudServices)
        : base(crudServices)
    { }

    protected override IQueryable<QuestionGroup> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Translates);
    }

    protected override IQueryable<QuestionGroup> InjectFilter(IQueryable<QuestionGroup> query)
    {
        return query.Where(a => a.StateId == StateIdConst.ACTIVE);
    }
}
