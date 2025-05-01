using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices
{
    public static class SettlementAccountSourceSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<SettlementAccountSource> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.Code,
                        Text = x.Translates.AsQueryable().FirstOrDefault(SettlementAccountSourceTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
