using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.NewsServices
{
    public static class NewsSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<News> query)
        {
            return new SelectList<int>(query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.Translates.AsQueryable().FirstOrDefault(NewsTranslate.GetExpr(NewsTranslateColumn.title, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Title
                })
                .OrderByDescending(a => a.Value)
                .ThenBy(a => a.Text));
        }
    }
}
