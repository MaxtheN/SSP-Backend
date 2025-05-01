using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class AcademicDegreeRepository : BaseEntityRepository<int, AcademicDegree, CreateAcademicDegreeDlDto, UpdateAcademicDegreeDlDto>, IAcademicDegreeRepository
{
    public AcademicDegreeRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<AcademicDegree> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
