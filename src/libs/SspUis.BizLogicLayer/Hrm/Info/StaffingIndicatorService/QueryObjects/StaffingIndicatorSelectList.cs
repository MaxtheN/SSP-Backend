using WEBASE;
using WEBASE.Models;
using WEBASE.DependencyInjection;
using System.Linq;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices
{
    public static class StaffingIndicatorSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<StaffingIndicator> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(StaffingIndicatorTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
