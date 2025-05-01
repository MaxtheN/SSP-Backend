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
    public class NationalityRepository : BaseEntityRepository<int, Nationality, CreateNationalityDlDto, UpdateNationalityDlDto>, INationalityRepository
    {
        public NationalityRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        public Nationality ByWbCode(string wbCode)
        {
            if (wbCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.WbCode == wbCode);
        }

        protected override IQueryable<Nationality> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }

        protected override void CreateValidate(CreateNationalityDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Nationality entity, UpdateNationalityDlDto dto)
        {
            Validate(entity, dto);
        }

        protected void Validate<TDto>(Nationality entity, NationalityDlDto<TDto> dto)
            where TDto : NationalityDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(x => x.Id == entity.Id);
        }
    }
}
