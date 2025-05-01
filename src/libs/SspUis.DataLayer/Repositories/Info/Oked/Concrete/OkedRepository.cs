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
    public class OkedRepository : BaseEntityRepository<int, Oked, CreateOkedDlDto, UpdateOkedDlDto>, IOkedRepository
    {
        public OkedRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public Oked ByCode(string code)
        {
            return ByIdQuery().FirstOrDefault(x => x.Code == code);
        }

        protected override IQueryable<Oked> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates);
        }

        protected override void CreateValidate(CreateOkedDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Oked entity, UpdateOkedDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Oked entity, OkedDlDto<TDto> dto)
            where TDto : OkedDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
                AddError($"Окед с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));
        }

    }
}
