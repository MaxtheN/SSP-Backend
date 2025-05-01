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
    public class CitizenshipRepository : BaseEntityRepository<int, Citizenship, CreateCitizenshipDlDto, UpdateCitizenshipDlDto>, ICitizenshipRepository
    {
        public CitizenshipRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public Citizenship ByWbCode(string wbCode)
        {
            if (wbCode.NullOrEmpty())
                return null;
            return ByIdQuery().FirstOrDefault(a => a.WbCode == wbCode);
        }

        protected override IQueryable<Citizenship> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates);
        }

        protected override void CreateValidate(CreateCitizenshipDlDto dto)
        {
            Validate(null, dto);
        }
        protected override void UpdateValidate(Citizenship entity, UpdateCitizenshipDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Citizenship entity, CitizenshipDlDto<TDto> dto)
            where TDto : CitizenshipDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }

    }
}
