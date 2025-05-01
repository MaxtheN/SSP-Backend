using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class EmployeeSendStudySelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<EmployeeSendStudy> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber + " - " + a.DocOn
                })
                .OrderBy(a => a.Text)
            );
    }
}
