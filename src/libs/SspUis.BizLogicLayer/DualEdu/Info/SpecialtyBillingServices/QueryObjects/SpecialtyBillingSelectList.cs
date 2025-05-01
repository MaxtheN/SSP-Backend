using SspUis.DataLayer.EfClasses.DualEdu;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class SpecialtyBillingSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<SpecialtyBilling> query, int? instituteId = null)
    {
        return new SelectList<int>(
            query
                .Where(a => !instituteId.HasValue || a.OrganizationId == instituteId.Value)
                .Select(x => new SelectListItem<int>
                {
                    Value = x.Id,
                    OrderCode = x.Code,
                    Text = x.FullName,
                })
                .OrderBy(x => x.OrderCode)
                .ThenBy(x => x.Text));
    }
}