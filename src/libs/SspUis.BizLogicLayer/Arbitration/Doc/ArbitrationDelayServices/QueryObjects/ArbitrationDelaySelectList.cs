using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class ArbitrationDelaySelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<ArbitrationDelay> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber,
                    OrderCode = a.DocOn.ToString()
                })
                .OrderBy(a => a.Text)
            );
    }
}
