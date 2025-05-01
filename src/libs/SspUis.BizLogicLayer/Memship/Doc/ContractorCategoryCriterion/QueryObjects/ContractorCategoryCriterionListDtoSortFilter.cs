using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer;

public static class ContractorCategoryCriterionListDtoSortFilter
{
    public static IQueryable<ContractorCategoryCriterionListDto> SortFilter(this IQueryable<ContractorCategoryCriterionListDto> query
     , ContractorCategoryCriterionSortFilterOptions options)
    {
        if (options.StatusId.HasValue && options.StatusId != 0)
            query = query.Where(d => d.StatusId == options.StatusId);

        if (options.ContractorCategoryId.HasValue)
            query = query.Where(d => d.ContractorCategoryId == options.ContractorCategoryId);
 
        if (options.ExpirationDate.HasValue)
            query = query.Where(d => d.ExpirationDate == options.ExpirationDate);

        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()) ||
                                      a.ContractorCategory.Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
