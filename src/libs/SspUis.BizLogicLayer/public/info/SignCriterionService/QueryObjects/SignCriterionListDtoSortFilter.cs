using WEBASE.Models;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService;

public static class SignCriterionListDtoSortFilter
{
    public static IQueryable<SignCriterionListDto> SortFilter(this IQueryable<SignCriterionListDto> query, ISortFilterOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => a.State.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Position.ToLower().Contains(options.Search.ToLower()) ||
                                     a.ContractorCategory.ToLower().Contains(options.Search.ToLower()) ||
                                     a.ApplicationType.ToLower().Contains(options.Search.ToLower()) ||
                                     a.OrganizationGroup.ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
