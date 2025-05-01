using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class ContractorCategoryCriterionSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<ContractorCategoryCriterion> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber
                })
                .OrderBy(a => a.Text)
            );
    }
}
