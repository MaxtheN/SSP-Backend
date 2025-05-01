using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.BankServices;

namespace SspUis.BizLogicLayer
{
    public static class BankSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<BankListDto> query)
        {
            return new SelectList<int>(
                query
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Code + " - " + a.BankName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
