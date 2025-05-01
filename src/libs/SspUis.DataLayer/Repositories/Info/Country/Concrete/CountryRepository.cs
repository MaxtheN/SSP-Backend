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
using SspUis.DataLayer;

namespace SspUis.DataLayer.Repositories
{
    public class CountryRepository : BaseEntityRepository<int, Country, CreateCountryDlDto, UpdateCountryDlDto>, ICountryRepository
    {
        public CountryRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public Country ByWbCode(string wbCode)
        {
            if (wbCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(w => w.TextCode == wbCode);
        }

        protected override IQueryable<Country> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates);
        }

        protected override void CreateValidate(CreateCountryDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Country entity, UpdateCountryDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Country entity, CountryDlDto<TDto> dto)
            where TDto : CountryDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
                AddError($"Страна с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));
            if (query.ByStringCode(dto.TextCode, isIncludePassive: true).Any())
                AddError($"Страна с этим кодом ({dto.TextCode}) уже существует.", nameof(dto.TextCode));
        }

    }
}
