using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class DistrictSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<District> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
