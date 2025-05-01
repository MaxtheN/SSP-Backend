using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ServicePriceTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ServicePriceType> source)
        {
            return new SelectList<int>(source
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.OrderCode,
                    Text = a.Translates.AsQueryable()
                        .FirstOrDefault(ServicePriceTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                        .TranslateText ?? a.FullName,
                })
                .OrderBy(a => a.OrderCode)
                .ThenBy(a => a.Text));
        }
    }
}
