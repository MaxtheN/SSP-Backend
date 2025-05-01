using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class LanguageProficiencyRepository : BaseEntityRepository<int, LanguageProficiency, CreateLanguageProficiencyDlDto, UpdateLanguageProficiencyDlDto>, ILanguageProficiencyRepository
{
    public LanguageProficiencyRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<LanguageProficiency> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
