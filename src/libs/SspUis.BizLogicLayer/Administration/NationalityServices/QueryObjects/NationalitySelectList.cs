using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.NationalityServices
{
    public static class NationalitySelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Nationality> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        Text = x.Translates.AsQueryable().FirstOrDefault(NationalityTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
