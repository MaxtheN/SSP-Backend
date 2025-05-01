using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class EducationItemRepository : BaseEntityRepository<int, EducationItem, CreateEducationItemDlDto, UpdateEducationItemDlDto>, IEducationItemRepository
{
    public EducationItemRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<EducationItem> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
