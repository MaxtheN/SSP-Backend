using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Appeal;

public static class AppealApplicationSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<AppealApplicationListDto> query)
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
