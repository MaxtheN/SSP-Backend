using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ApplicationSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<ApplicationServices.ApplicationListDto> query)
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
}
