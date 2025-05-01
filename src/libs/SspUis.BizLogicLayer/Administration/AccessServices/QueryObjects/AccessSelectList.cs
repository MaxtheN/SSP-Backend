using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE;

namespace SspUis.BizLogicLayer
{
    public static class AccessSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Accessibility> query)
        {
            return new SelectList<int>(
                query
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.Code,
                        Text = a.FullName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
