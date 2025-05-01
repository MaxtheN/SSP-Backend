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
    public class NewsRepository : BaseEntityRepository<int, News, CreateNewsDlDto, UpdateNewsDlDto>, INewsRepository
    {
        public NewsRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public void AddViewCount(int id)
        {
            var ent = ById(id);
            ent.ViewCount++;
            Context.Entry(ent).State = EntityState.Modified;
        }

        protected override IQueryable<News> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates)
                                 .Include(a => a.Image);
        }

        protected override void CreateValidate(CreateNewsDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(News entity, UpdateNewsDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(News entity, NewsDlDto<TDto> dto)
           where TDto : NewsDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
