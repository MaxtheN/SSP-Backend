using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.FixedMinimumValueServices
{
    public static class FixedMinimumValueSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<FixedMinimumValue> query)
        {
            return new SelectList<long>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<long>
                    {
                        Value = x.Id,
                        OrderCode = x.DateOn.ToString(),
                    })
                    .OrderBy(x => x.Value)
                    .ThenBy(x => x.OrderCode)
                );
        }
    }
}
