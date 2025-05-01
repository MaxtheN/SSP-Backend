using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppErrorServices;

public static class AppErrorListDtoSortFilter
{
    public static IQueryable<AppErrorListDto> SortFilter(this IQueryable<AppErrorListDto> query, SortFilterPageOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => a.RequestPath.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Title.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Host.ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderBy($"{nameof(AppErrorListDto.Id)} {options.OrderType}");

        return query;
    }
}
