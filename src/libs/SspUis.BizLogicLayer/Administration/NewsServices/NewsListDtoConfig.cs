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

namespace SspUis.BizLogicLayer.NewsServices
{
    public class NewsListDtoConfig : PerDtoConfig<NewsListDto, News>
    {
        public override Action<IMappingExpression<News, NewsListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.Title, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NewsTranslate.GetExpr(NewsTranslateColumn.title, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Title))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
             .ForMember(x => x.ShortContent, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NewsTranslate.GetExpr(NewsTranslateColumn.short_content, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortContent));
    }
}
