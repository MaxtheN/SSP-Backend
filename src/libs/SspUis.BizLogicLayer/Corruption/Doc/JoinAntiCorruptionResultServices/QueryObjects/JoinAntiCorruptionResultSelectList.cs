using System.Linq;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Models;
namespace SspUis.BizLogicLayer.Corruption;

public static class JoinAntiCorruptionResultSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<JoinAntiCorruptionResult> query)
    {
        return new SelectList<long>(
            query
                .Select(x => new SelectListItem<long>
                {
                    Value = x.Id,
                    OrderCode = x.Organization.FullName
                })
                .OrderBy(x => x.OrderCode)
            );
    }
}
