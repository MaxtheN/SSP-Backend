using AutoMapper;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.NewsServices
{
    public class NewsViewDto : ILinkToEntity<News>
    {
        public int ViewCount { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }
        public DateTime Date { get; set; }
        public NewsImageDlDto Image { get; set; }
        public List<TagDto> Tags { get; internal set; } = new();
    }

    public class NewsViewDtoConfig : PerDtoConfig<NewsViewDto, News>
    {
        public override Action<IMappingExpression<News, NewsViewDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.Tags, x => x.MapFrom(ent => ent.Tags.Select(a => a.Tag)))
            .ForMember(x => x.Title, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NewsTranslate.GetExpr(NewsTranslateColumn.title, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Title))
            .ForMember(x => x.ShortContent, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NewsTranslate.GetExpr(NewsTranslateColumn.short_content, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortContent))
            .ForMember(x => x.Content, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NewsTranslate.GetExpr(NewsTranslateColumn.content, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Content))
            ;
    }
}
