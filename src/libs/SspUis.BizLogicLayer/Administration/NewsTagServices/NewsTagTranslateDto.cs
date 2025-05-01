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

//namespace SspUis.BizLogicLayer.NewsTagServices
//{
//    public class NewsTagTranslateDto : NewsTagTranslateDlDto, ILinkToEntity<NewsTagTranslate>
//    {
//        public string Language { get; set; }
//    }
//    public class NewsTagTranslateDtoConfig : PerDtoConfig<NewsTagTranslateDto, NewsTagTranslate>
//    {
//        public override Action<IMappingExpression<NewsTagTranslate, NewsTagTranslateDto>> AlterReadMapping => cfg => cfg
//               .IncludeBase<NewsTagTranslate, NewsTagTranslateDlDto>()
//               .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
//    }
//}
