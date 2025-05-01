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

namespace SspUis.BizLogicLayer.NewsTagServices
{
    public class NewsTagListDtoConfig : PerDtoConfig<NewsTagListDto, NewsTag>
    {
        //public override Action<IMappingExpression<NewsTag, NewsTagListDto>> AlterReadMapping => cfg => cfg
        //    .ForMember(x => x.News, x => x.MapFrom(ent => ent.News.Translates.AsQueryable()
        //    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.News));
    }
}
