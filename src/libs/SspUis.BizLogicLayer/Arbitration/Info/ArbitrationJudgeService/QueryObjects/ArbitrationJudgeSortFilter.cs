using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public static class ArbitrationJudgeListDtoSortFilter
{
    public static IQueryable<ArbitrationJudgeListDto> SortFilter(this IQueryable<ArbitrationJudgeListDto> query, ArbitrationJudgeSortFilterOptions options)
    {
        if (options.RegionId.HasValue)
            query = query.Where(x => x.RegionId == options.RegionId);

        if (options.HasSearch())
            query = query.Where(a => a.FirstName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.LastName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.MiddleName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Id.ToString().ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
