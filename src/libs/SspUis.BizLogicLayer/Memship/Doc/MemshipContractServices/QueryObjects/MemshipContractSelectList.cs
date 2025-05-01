using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public static class MemshipContractSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<MemshipContract> query)
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
