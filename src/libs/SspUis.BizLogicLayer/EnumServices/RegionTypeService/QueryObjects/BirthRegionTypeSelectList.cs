using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class BirthRegionTypeSelectList
{
    public static SelectList<int> AsSelectList1(this IQueryable<Person> source)
    {
        return new SelectList<int>(source.Include(p => p.BirthRegion).ThenInclude(r => r.Translates)
            .Where(p => p.BirthRegion != null)
            .Select(p => new SelectListItem<int>
            {
                Value = p.BirthRegion.Id,
                OrderCode = p.BirthRegion.OrderCode,
                Text = p.BirthRegion.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? p.BirthRegion.FullName
            }).Distinct());
    }
}
