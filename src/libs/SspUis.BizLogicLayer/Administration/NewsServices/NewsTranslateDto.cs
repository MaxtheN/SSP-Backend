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

namespace SspUis.BizLogicLayer.NewsServices
{
    public class NewsTranslateDto : NewsTranslateDlDto, ILinkToEntity<NewsTranslate>
    {
        public string Language { get; set; }
    }
    public class NewsTranslateDtoConfig : PerDtoConfig<NewsTranslateDto, NewsTranslate>
    {
        public override Action<IMappingExpression<NewsTranslate, NewsTranslateDto>> AlterReadMapping => cfg => cfg
               .IncludeBase<NewsTranslate, NewsTranslateDlDto>()
               .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
