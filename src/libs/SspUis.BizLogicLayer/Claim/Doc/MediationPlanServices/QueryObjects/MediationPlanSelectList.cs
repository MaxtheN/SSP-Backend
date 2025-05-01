using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public static class MediationPlanSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<MediationPlanListDto> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber + " - " + a.DocOn
                })
                .OrderBy(a => a.Text)
            );
    }
}
