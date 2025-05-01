using System.Linq;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;
namespace SspUis.BizLogicLayer.Hrm;

public static class AppointEmployeeSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<AppointEmployee> query)
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
