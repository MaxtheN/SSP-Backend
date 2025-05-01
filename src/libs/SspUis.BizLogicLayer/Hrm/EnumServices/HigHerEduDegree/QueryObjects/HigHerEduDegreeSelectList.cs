using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class EmployeeHigherEduDegreeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<EmployeeHigherEduDegree> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
                .FirstOrDefault(EmployeeHigherEduDegreeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
