using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PositionCategoryServices
{
    public static class PositionCategorySelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<PositionCategory> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        //OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(PositionCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.Value)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
