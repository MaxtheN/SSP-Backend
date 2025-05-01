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
    public class NewsTagRepository : BaseEntityRepository<int, NewsTag,CreateNewsTagDlDto,UpdateNewsTagDlDto>, INewsTagRepository
    {
        public NewsTagRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<NewsTag> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Tag);
        }

        protected override void CreateValidate(CreateNewsTagDlDto dto)
        {
            Validate(null,dto);
        }

        protected override void UpdateValidate(NewsTag entity, UpdateNewsTagDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(NewsTag entity, NewsTagDlDto<TDto> dto)
           where TDto : NewsTagDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
