using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.DualEducationTypeServices
{
    public static class DualEducationTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<DualEducationType> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(DualEducationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}