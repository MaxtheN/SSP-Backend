using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class TimesheetSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<Timesheet> query)
    {
        return new SelectList<long>(
            query
                .Select(x => new SelectListItem<long>
                {
                    Value = x.Id,
                    OrderCode = x.Organization.FullName
                })
                .OrderBy(x => x.OrderCode)
            );
    }
}
