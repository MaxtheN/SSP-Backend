//using AutoMapper;
//using GenericServices;
//using GenericServices.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SspUis.DataLayer.EfClasses;
//using SspUis.DataLayer.Repositories;

//namespace SspUis.BizLogicLayer.TagServices
//{
//    public class TagTranslateDto : TagTranslateDlDto, ILinkToEntity<TagTranslate>
//    {
//        public string Language { get; set; }
//    }
//    public class TagTranslateDtoConfig : PerDtoConfig<TagTranslateDto, TagTranslate>
//    {
//        public override Action<IMappingExpression<TagTranslate, TagTranslateDto>> AlterReadMapping => cfg => cfg
//               .IncludeBase<TagTranslate, TagTranslateDlDto>()
//               .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
//    }
//}
