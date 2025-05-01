using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.NewsTagServices
{
    public static class NewsTagSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<NewsTag> query)
        {
            return new SelectList<int>(query
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    //Text = a.Translates.AsQueryable().FirstOrDefault(NewsTagTranslate.GetExpr(NewsTagTranslateColumn.title, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Name
                })
                .OrderByDescending(a => a.Value)
                .ThenBy(a => a.Text));
        }
    }
}
