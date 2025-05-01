using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class TariffScaleCoefRepository : BaseEntityRepository<int, TariffScaleCoef, CreateTariffScaleCoefDlDto, UpdateTariffScaleCoefDlDto>, ITariffScaleCoefRepository
    {
        public TariffScaleCoefRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }
        protected override IQueryable<TariffScaleCoef> ByIdQuery()
            => AllAsQueryable.Include(x => x.Translates)
            .Include(x => x.Tables);
    }
}
