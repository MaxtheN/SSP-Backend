using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public static class DebtSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<Debt> query)
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
