using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.VideoCategoryServices
{
    public class VideoCategoryTranslateDto : VideoCategoryTranslateDlDto, ILinkToEntity<VideoCategoryTranslate>
    {
        public string Language { get; set; }
    }
    public class VideoCategoryTranslateDtoConfig : PerDtoConfig<VideoCategoryTranslateDto, VideoCategoryTranslate>
    {
        public override Action<IMappingExpression<VideoCategoryTranslate, VideoCategoryTranslateDto>> AlterReadMapping => cfg => cfg
               .IncludeBase<VideoCategoryTranslate, VideoCategoryTranslateDlDto>()
               .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
