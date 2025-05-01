using System.Linq;

using SspUis.DataLayer.EfClasses.Hrm;

using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class RoundingTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<RoundingType> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
                .FirstOrDefault(RoundingTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
