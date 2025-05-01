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
    public class TagRepository : BaseEntityRepository<int, Tag, CreateTagDlDto, UpdateTagDlDto>, ITagRepository
    {
        public TagRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public IQueryable<Tag> ByNames(List<string> tags)
        {
            var names = tags.ToArray();
            return AllAsQueryable.Where(a => names.Contains(a.Name));
        }

        public IQueryable<TDto> ByNames<TDto>(List<string> tags) where TDto : class
        {
            var names = tags.ToArray();
            return ReadAsNoTracked<TDto>(a => names.Contains(a.Name));
        }

        protected override IQueryable<Tag> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.NewsTags);
        }

        protected override void CreateValidate(CreateTagDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Tag entity, UpdateTagDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Tag entity, TagDlDto<TDto> dto)
           where TDto : TagDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
