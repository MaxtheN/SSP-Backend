using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public static class ClaimResponsibleTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<ClaimResponsibleType> source, int? langId)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
            .FirstOrDefault(ClaimResponsibleTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, langId ?? 3)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
