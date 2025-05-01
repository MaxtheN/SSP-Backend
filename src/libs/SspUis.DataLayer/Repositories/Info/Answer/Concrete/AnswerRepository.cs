using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class AnswerRepository : BaseEntityRepository<long, Answer, CreateAnswerDlDto, UpdateAnswerDlDto>, IAnswerRepository
{
    public AnswerRepository(ICrudServices crudServices)
        : base(crudServices)
    { }

    public override Answer Create(CreateAnswerDlDto createDto, Action<Answer> validation = null)
    {
        return base.Create(createDto, validation);
    }
    protected override IQueryable<Answer> ByIdQuery()
    {
        return AllAsQueryable.Include(a => a.Translates);
    }

    protected override IQueryable<Answer> InjectFilter(IQueryable<Answer> query)
    {
        return query.Where(a => a.StateId == StateIdConst.ACTIVE);
    }
}
