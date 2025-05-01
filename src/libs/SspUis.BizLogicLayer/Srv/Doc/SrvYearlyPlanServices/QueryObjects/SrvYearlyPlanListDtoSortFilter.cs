using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public static class SrvYearlyPlanListDtoSortFilter
{
    public static IQueryable<SrvYearlyPlanListDto> SortFilter(this IQueryable<SrvYearlyPlanListDto> query
     , SrvYearlyPlanSortFilterOptions options)
    {
        if (options.StatusId.HasValue)
            query.Where(a => a.StatusId == options.StatusId);

        if (options.OrganizationId.HasValue)
            query.Where(a => a.OrganizationId == options.OrganizationId);



        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()));
                                      

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
