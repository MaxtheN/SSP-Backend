using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public static class AppealDescriptionSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<AppealDescription> query, bool hasParent)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Where(a => a.HasParent == hasParent)
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.OrderCode,
                    Text = a.Translates.AsQueryable()
                    .FirstOrDefault(AppealDescriptionTranslate.GetExpr(TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id)
                    ).TranslateText ?? a.FullName
                })
                .OrderBy(a => a.Text)
            );
    }
}
