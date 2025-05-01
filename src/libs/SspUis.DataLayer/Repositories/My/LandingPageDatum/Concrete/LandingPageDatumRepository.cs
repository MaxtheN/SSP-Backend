using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class LandingPageDatumRepository : BaseEntityRepository<int, LandingPageDatum,CreateLandingPageDatumDlDto,UpdateLandingPageDatumDlDto>, ILandingPageDatumRepository
    {
        public LandingPageDatumRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<LandingPageDatum> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }

        protected override void CreateValidate(CreateLandingPageDatumDlDto dto)
        {
            Validate(null,dto);
        }

        protected override void UpdateValidate(LandingPageDatum entity, UpdateLandingPageDatumDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(LandingPageDatum entity, LandingPageDatumDlDto<TDto> dto)
           where TDto : LandingPageDatumDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
