using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Kpi;

public static class KpiPlanForEmployeeSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<KpiPlanForEmployee> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber
                })
                .OrderBy(a => a.Text)
            );
    }
}
