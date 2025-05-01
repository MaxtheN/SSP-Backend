using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class VideoLessonDlDto<TDto> : EntityDto<TDto, VideoLesson>
        where TDto : VideoLessonDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        [LocalizedStringLength(50)]
        public string Number { get; set; }
        public int CategoryId { get; set; }
        public string Theme { get; set; }
        [LocalizedStringLength(500)]
        public string Tag { get; set; }
        public string Uri { get; set; }
        public override VideoLesson CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }
    }
}
