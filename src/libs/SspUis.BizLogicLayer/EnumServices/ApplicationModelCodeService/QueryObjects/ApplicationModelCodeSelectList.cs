using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ApplicationModelCodeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ApplicationModelCode> source)
        {
            return new SelectList<int>(source
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.ShortName,
                }));
        }
    }
}
