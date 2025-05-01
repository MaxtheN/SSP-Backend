using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class MassPlannedCalculationSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<MassPlannedCalculation> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber + " - " + a.StartOn + " дан " + a.EndOn + " гача"
                })
                .OrderBy(a => a.Text)
            );
    }
}
