using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PositionClassificationServices
{
    public static class PositionClassificationSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<PositionClassification> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        //OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(PositionClassificationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.Value)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
