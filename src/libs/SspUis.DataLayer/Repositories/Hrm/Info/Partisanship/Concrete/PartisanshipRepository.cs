using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PartisanshipRepository : BaseEntityRepository<int, Partisanship, CreatePartisanshipDlDto, UpdatePartisanshipDlDto>, IPartisanshipRepository
    {
        public PartisanshipRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }
        protected override IQueryable<Partisanship> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates);
    }
}
