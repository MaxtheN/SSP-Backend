using System.Linq;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class AppealTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<AppealType> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.FullName
            //Text = a.Translates.AsQueryable()
            //    .FirstOrDefault(AppealFormatTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
