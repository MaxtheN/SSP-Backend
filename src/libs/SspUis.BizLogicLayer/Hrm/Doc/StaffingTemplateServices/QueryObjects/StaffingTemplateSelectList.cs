using System.Linq;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public static class StaffingTemplateSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<StaffingTemplate> query)
        {
            return new SelectList<long>(
                query
                    .Select(a => new SelectListItem<long>
                    {
                        Value = a.Id,
                        Text = a.DocNumber + " - " + a.DocOn
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
