using SspUis.DataLayer.EfClasses.Kpi;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class KpiRatingEmployeeSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<KpiRatingEmployee> query)
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
