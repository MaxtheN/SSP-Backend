using System.Linq;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption;

public static class JoinAntiCorruptionResultTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<JoinAntiCorruptionResultType> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
                .FirstOrDefault(JoinAntiCorruptionResultTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
