using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService
{
    public static class SignCriterionSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<SignCriterion> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
