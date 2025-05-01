using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption;

public static class JoinAntiCorruptionResultListDtoSortFilter
{
    public static IQueryable<JoinAntiCorruptionResultListDto> SortFilter(this IQueryable<JoinAntiCorruptionResultListDto> query
       , JoinAntiCorruptionResultSortFilterOptions options)
    {

        if (options.HasSearch())
            query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower())
                                  || a.ChairmenFio.ToLower().Contains(options.Search.ToLower())
                                  || a.Member1Fio.ToLower().Contains(options.Search.ToLower())
                                  || a.Member2Fio.ToLower().Contains(options.Search.ToLower())
                                  || a.Member3Fio.ToLower().Contains(options.Search.ToLower())
                                  || a.Member4Fio.ToLower().Contains(options.Search.ToLower()));

        if (options.FromDocDate.HasValue)
            query = query.Where(a => options.FromDocDate.Value <= a.DocOn);

        if (options.ToDocDate.HasValue)
            query = query.Where(a => a.DocOn <= options.ToDocDate.Value);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
