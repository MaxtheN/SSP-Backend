using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public static class OpfSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<Opf> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
            .FirstOrDefault(OpfTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}