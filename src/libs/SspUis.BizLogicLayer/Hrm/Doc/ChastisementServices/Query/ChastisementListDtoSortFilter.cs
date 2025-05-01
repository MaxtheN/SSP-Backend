using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class ChastisementListDtoSortFilter
{
    public static IQueryable<ChastisementListDto> SortFilter(this IQueryable<ChastisementListDto> query
    , ChastisementSortFilterOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()));
        if (options.StatusId.HasValue)
            query = query.Where(a => a.StatusId == options.StatusId);
        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
    public static IQueryable<UnpaidChastisementListDto> SortFilter(this IQueryable<UnpaidChastisementListDto> query, UnpaidChastisementSortFilterPageOptions options)
    {
        if (options.StartDate.HasValue)
            query = query.Where(a => a.DocOn >= options.StartDate);

        if (options.EndDate.HasValue)
            query = query.Where(a => a.DocOn <= options.EndDate);

        if (options.HasSearch())
            query = query.Where(a => ("" + a.Employee.ToLower()).Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
