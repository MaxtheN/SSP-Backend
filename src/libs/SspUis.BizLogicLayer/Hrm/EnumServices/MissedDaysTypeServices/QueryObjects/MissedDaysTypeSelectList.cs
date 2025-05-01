using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public static class MissedDaysTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<MissedDaysType> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                OrderCode = a.OrderCode,
                Text = a.Translates.AsQueryable()
                .FirstOrDefault(MissedDaysTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
            }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
        }
    }
}
