using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;
namespace SspUis.BizLogicLayer.CustomJobServices;
public static class CustomJobListDtoSortFilter
{
    public static IQueryable<CustomJobListDto> SortFilter(this IQueryable<CustomJobListDto> query, ISortFilterOptions options)
    {
        if (options is CustomJobSortFilterOptions)
        {
            var _options = (CustomJobSortFilterOptions)options;

        }
        if (options.HasSearch())
            query = query.Where(a => a.Id.ToString().ToLower().Contains(options.Search.ToLower()));
        if (options.HasSort())
            query = query.OrderBy(options.SortBy, options.OrderType);
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
