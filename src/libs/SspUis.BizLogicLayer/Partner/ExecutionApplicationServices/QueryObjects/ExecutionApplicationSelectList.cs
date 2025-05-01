using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices;

public static class ExecutionApplicationSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<ExecutionApplicationListDto> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.Status + " - " + a.DocOn
                })
                .OrderBy(a => a.Text)
            );
    }
}
