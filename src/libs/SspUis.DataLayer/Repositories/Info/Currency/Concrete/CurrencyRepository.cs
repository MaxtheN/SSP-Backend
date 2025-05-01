using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CurrencyRepository : BaseEntityRepository<int, Currency, CreateCurrencyDlDto, UpdateCurrencyDlDto>, ICurrencyRepository
    {
        public CurrencyRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }
        protected override IQueryable<Currency> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }

        protected override void CreateValidate(CreateCurrencyDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Currency entity, UpdateCurrencyDlDto dto)
        {
            Validate(entity, dto);
        }

        protected void Validate<TDto>(Currency entity, CurrencyDlDto<TDto> dto)
            where TDto : CurrencyDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(x => x.Id == entity.Id);
        }
    }
}
