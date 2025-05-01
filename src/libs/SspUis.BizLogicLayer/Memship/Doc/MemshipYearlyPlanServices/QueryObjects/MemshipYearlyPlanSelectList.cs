using SspUis.DataLayer.EfClasses.Memship;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public static class MemshipYearlyPlanSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<MemshipYearlyPlan> query)
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
