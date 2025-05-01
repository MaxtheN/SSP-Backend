using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;
using WEBASE;

namespace SspUis.BizLogicLayer
{
    public static class IndicatorDistrictSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<IndicatorDistrict> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.FullName
                    })
                    .OrderBy(a => a.OrderCode)
                );
        }
    }
}
