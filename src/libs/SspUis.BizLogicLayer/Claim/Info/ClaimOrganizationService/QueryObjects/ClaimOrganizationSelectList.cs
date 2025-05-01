using WEBASE;
using WEBASE.Models;
using WEBASE.DependencyInjection;
using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationServices
{
    public static class ClaimOrganizationSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ClaimOrganization> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(ClaimOrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
