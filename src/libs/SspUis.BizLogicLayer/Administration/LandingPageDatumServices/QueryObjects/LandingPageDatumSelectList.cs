using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public static class LandingPageDatumSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<LandingPageDatum> query)
        {
            return new SelectList<int>(query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.OrderCode,
                    Text = a.Translates.AsQueryable().FirstOrDefault(LandingPageDatumTranslate.GetExpr(LandingPageDatumTranslateColumn.label, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Label
                })
                .OrderBy(a => a.OrderCode)
                .ThenBy(a => a.Text));
        }
    }
}
