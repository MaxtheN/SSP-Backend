using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.Memship;

public static class MemshipPaymentOrderSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<MemshipPaymentOrder> query)
    {
        return new SelectList<long>(
            query
                .Select(x => new SelectListItem<long>
                {
                    Value = x.Id,
                    OrderCode = x.Amount.ToString(),
                    Text = x.Contractor.FullName,
                })
                .OrderBy(x => x.OrderCode)
                .ThenBy(x => x.Text)
            );
    }
}
