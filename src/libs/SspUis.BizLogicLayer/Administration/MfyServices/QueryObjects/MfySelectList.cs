using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class MfySelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<Mfy> query)
        {
            return new SelectList<long>(
                query
                    .IsActive()              
                    .Select(a => new SelectListItem<long>
                    {
                        Value = a.Id,
                        OrderCode = a.ExternalId.ToString(),
                        Text = a.FullName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
