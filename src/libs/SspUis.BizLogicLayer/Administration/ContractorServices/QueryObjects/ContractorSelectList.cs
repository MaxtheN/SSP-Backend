using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer
{
    public static class ContractorSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<Contractor> query)
        {
            return new SelectList<long>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<long>
                    {
                        Value = a.Id,
                        OrderCode = a.Pinfl,
                        Text = a.FullName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
