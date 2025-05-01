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
    public class VideoLessonRepository : BaseEntityRepository<long, VideoLesson, CreateVideoLessonDlDto, UpdateVideoLessonDlDto>, IVideoLessonRepository
    {
        public VideoLessonRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }
        public VideoLesson ByNumber(string number)
        {
            return ByIdQuery().FirstOrDefault(a => a.Number == number);
        }
        protected override void CreateValidate(CreateVideoLessonDlDto dto)
        {
            Validate(null, dto);
        }
        protected override void UpdateValidate(VideoLesson entity, UpdateVideoLessonDlDto dto)
        {
            Validate(entity, dto);
        }
        private void Validate<TDto>(VideoLesson entity, VideoLessonDlDto<TDto> dto)
            where TDto : VideoLessonDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            //if (query.ByNumber(dto.Number, isIncludePassive: true).Any())
            //    AddError($"Видео уроков с этим номер ({dto.Number}) уже существует.", nameof(dto.Number));
        }
    }
}
