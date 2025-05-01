using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public class VideoLessonDtoConfig : PerDtoConfig<VideoLessonDto, VideoLesson>
    {
        public override Action<IMappingExpression<VideoLesson, VideoLessonDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.Category, x => x.MapFrom(ent => ent.Category.Translates.AsQueryable()
                .FirstOrDefault(VideoCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Category.FullName))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName));
    }
}
