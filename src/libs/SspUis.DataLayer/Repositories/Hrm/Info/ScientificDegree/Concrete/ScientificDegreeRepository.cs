using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ScientificDegreeRepository : BaseEntityRepository<int, ScientificDegree, CreateScientificDegreeDlDto, UpdateScientificDegreeDlDto>, IScientificDegreeRepository
{
    public ScientificDegreeRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<ScientificDegree> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
