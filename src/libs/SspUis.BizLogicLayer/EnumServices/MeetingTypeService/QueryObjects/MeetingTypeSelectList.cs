using Microsoft.EntityFrameworkCore;
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
    public static class MeetingTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<MeetingType> source)
        {
            return new SelectList<int>(source
                .Include(a => a.Translates)
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.Translates.AsQueryable().FirstOrDefault(MeetingTypeTranslate.GetExpr(TranslateColumn.full_name)).TranslateText ?? a.FullName,
                }));
        }
    }
}
