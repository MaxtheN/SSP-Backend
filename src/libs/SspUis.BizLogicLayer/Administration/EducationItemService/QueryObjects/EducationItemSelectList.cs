using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class EducationItemSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<EducationItem> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(x => new SelectListItem<int>
                {
                    Value = x.Id,
                    //OrderCode = x.OrderCode,
                    Text = x.Translates.AsQueryable().FirstOrDefault(EducationItemTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                })
                .OrderBy(x => x.Value)
                .ThenBy(x => x.Text)
            );
    }
}
