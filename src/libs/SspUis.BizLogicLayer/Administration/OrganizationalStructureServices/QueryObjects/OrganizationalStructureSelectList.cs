using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class OrganizationalStructureSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<OrganizationalStructure> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new OrganizationalStructureSelectListItem
                    {
                        Value = a.Id,
                        Code = a.Code,
                        Name = a.Translates.AsQueryable().FirstOrDefault(OrganizationalStructureTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
                        Text = a.Code + "-" + (a.Translates.AsQueryable().FirstOrDefault(OrganizationalStructureTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName)
                    })
                    .OrderBy(a => a.Code)
                );
        }

        public class OrganizationalStructureSelectListItem : SelectListItem<int>
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}
