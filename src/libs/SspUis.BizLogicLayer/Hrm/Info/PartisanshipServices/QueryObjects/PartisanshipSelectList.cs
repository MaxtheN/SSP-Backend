using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices
{
    public static class PartisanshipSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Partisanship> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        Text = x.Translates.AsQueryable().FirstOrDefault(PartisanshipTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.Value)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
