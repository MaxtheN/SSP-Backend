using System.Linq;

using SspUis.DataLayer.EfClasses;

using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class TimesheetIndicatorSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<TimesheetIndicator> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
                .FirstOrDefault(TimesheetIndicatorTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
