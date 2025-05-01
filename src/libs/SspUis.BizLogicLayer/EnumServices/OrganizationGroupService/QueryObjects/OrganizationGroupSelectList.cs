using DocumentFormat.OpenXml.Wordprocessing;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class OrganizationGroupSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<OrganizationGroup> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                OrderCode = a.Code,
                Text = a.Translates.AsQueryable().FirstOrDefault(OrganizationGroupTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
            }));
        }
    }
}
