using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public static class EmploymentTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<EmploymentType> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                OrderCode = a.Code,
                Text = a.Translates.AsQueryable()
                .FirstOrDefault(EmploymentTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
            }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
        }
    }
}
