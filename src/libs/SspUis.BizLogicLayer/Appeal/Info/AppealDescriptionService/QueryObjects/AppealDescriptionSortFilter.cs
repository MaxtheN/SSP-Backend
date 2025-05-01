using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public static class AppealDescriptionListDtoSortFilter
{
    public static IQueryable<AppealDescriptionListDto> SortFilter(this IQueryable<AppealDescriptionListDto> query, ISortFilterOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Code.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Details.ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
