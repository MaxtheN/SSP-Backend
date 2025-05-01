using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class StatusSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Status> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name)).TranslateText ?? a.FullName,
            }));
        }
    }
}
