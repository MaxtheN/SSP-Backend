using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public static class AppealTypeArriveSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<AppealTypeArrive> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.OrderCode,
                    Text = a.Translates
                    .AsQueryable()
                    .FirstOrDefault(
                        AppealTypeArriveTranslate.GetExpr(TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                })
                .OrderBy(a => a.Text)
            );
    }
}
