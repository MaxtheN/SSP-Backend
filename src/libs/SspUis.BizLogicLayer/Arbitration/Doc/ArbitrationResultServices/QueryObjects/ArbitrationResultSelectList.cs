using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class ArbitrationResultSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<ArbitrationResult> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber
                })
                .OrderBy(a => a.Text)
            );
    }
}
