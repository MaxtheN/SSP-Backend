using SspUis.BizLogicLayer.Claim;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class SrvApplicationSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<SrvApplicationListDto> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.Application.DocNumber + " - " + a.DocOn
                })
                .OrderBy(a => a.Value)
            );
    }
}
