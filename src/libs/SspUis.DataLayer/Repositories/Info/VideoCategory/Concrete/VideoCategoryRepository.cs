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
    public class VideoCategoryRepository : BaseEntityRepository<int, VideoCategory, CreateVideoCategoryDlDto, UpdateVideoCategoryDlDto>, IVideoCategoryRepository
    {
        public VideoCategoryRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<VideoCategory> ByIdQuery()
        {
            return AllAsQueryable.Include(x => x.Translates);
        }

        protected override void CreateValidate(CreateVideoCategoryDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(VideoCategory entity, UpdateVideoCategoryDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(VideoCategory entity, VideoCategoryDlDto<TDto> dto)
           where TDto : VideoCategoryDlDto<TDto>
        {
            var query = DbSet.AsQueryable();
            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
