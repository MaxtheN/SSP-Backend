using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public static class PlannedCalculationListDtoSortFilter
{
    public static IQueryable<PlannedCalculationListDto> SortFilter(this IQueryable<PlannedCalculationListDto> query
     , PlannedCalculationSortFilterOptions options)
    {
        if(options.StatusId.HasValue)
            query.Where(d => d.StatusId == options.StatusId);

        if(options.HasSearch())
            query = query.Where(a =>
                   a.DocNumber.ToLower().Contains(options.Search.ToLower())
                || a.Organization.ToLower().Contains(options.Search.ToLower())
            );
        query.ToList();
        if(options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);
        return query;
    }
}
