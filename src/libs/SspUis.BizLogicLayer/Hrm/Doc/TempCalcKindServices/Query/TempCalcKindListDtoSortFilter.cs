using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public static class TempCalcKindListDtoSortFilter
{
    public static IQueryable<TempCalcKindListDto> SortFilter(this IQueryable<TempCalcKindListDto> query
    , TempCalcKindSortFilterOptions options)
    {
        if(options.StatusId.HasValue)
            query = query.Where(d => d.StatusId == options.StatusId);

        if(options.HasSearch())
            query = query.Where(a =>
                   a.DocNumber.ToLower().Contains(options.Search.ToLower())
                || a.DocNumber.ToLower().Contains(options.Search.ToLower())
            );
        query.ToList();
        if(options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);
        return query;
    }
}
