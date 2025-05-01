using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE;
using SspUis.DataLayer;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public static class DepartmentSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Department> query, int? selectedOrganizationId)
        {
            return new SelectList<int>(
                   query
                       .IsActive()
                       .Where(a => a.OrganizationId == selectedOrganizationId)
                       .Select(x => new DepartmentItemDto
                       {
                           
                           Value = x.Id,
                           OrderCode = x.OrderCode,
                           Code = x.Code,
                           IndexCode = x.IndexCode,
                           Text = x.Translates.AsQueryable().FirstOrDefault(DepartmentTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                       })
                       .OrderBy(x => x.Value)
                       .ThenBy(x => x.OrderCode)
                   );
        }
        private class DepartmentItemDto : SelectListItem<int>
        {
            public string? IndexCode { get; set; }
            public long? Code { get; set; }

        }
    }
}


