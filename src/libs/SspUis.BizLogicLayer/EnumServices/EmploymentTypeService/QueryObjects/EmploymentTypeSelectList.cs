using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class EmploymentTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<EmploymentType> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.Translates.AsQueryable().FirstOrDefault(EmploymentTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
                OrderCode = a.Code
            }));
        }
    }
}
