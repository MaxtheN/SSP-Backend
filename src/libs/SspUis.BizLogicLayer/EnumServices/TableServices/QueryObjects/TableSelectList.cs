using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class TableSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Table> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.FullName,
            }));
        }
    }
}
