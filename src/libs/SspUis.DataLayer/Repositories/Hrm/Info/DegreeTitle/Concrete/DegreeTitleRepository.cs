using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DegreeTitleRepository : BaseEntityRepository<int, DegreeTitle, CreateDegreeTitleDlDto, UpdateDegreeTitleDlDto>, IDegreeTitleRepository
{
    public DegreeTitleRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<DegreeTitle> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
