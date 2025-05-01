using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.TagServices
{
    public static class TagSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Tag> query)
        {
            return new SelectList<int>(query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    //Text = a.Translates.AsQueryable().FirstOrDefault(TagTranslate.GetExpr(TagTranslateColumn.title, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Name
                })
                .OrderByDescending(a => a.Value)
                .ThenBy(a => a.Text));
        }
    }
}
