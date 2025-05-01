using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class TariffScaleRepository : BaseEntityRepository<int, TariffScale, CreateTariffScaleDlDto, UpdateTariffScaleDlDto>, ITariffScaleRepository
    {
        public TariffScaleRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<TariffScale> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates)
            .Include(x => x.Tables);
    }
}
