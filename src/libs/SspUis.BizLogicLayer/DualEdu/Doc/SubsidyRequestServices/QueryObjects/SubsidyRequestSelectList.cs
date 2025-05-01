using System.Linq;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class SubsidyRequestSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<SubsidyRequest> query)
    {
        return new SelectList<long>(
            query
                .Select(x => new SelectListItem<long>
                {
                    Value = x.Id,
                    //OrderCode = x.Amount.ToString(),
                    Text = x.DocNumber,
                })
                .OrderBy(x => x.OrderCode)
                .ThenBy(x => x.Text)
            );
    }
}
