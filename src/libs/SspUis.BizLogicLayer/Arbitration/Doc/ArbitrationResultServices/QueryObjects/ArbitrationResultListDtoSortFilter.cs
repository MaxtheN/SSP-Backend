using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public static class ArbitrationResultListDtoSortFilter
{
    public static IQueryable<ArbitrationResultListDto> SortFilter(
        this IQueryable<ArbitrationResultListDto> query,
        ArbitrationResultSortFilterOptions options)
    {
        if (options.FromDocDate.HasValue)
            query = query.Where(a => a.DocOn >= options.FromDocDate);

        if (options.ToDocDate.HasValue)
            query = query.Where(a => a.DocOn <= options.ToDocDate);

        if (options.HasSearch())
            query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower()) ||
                               (a.ResponsibleContractor.ToLower().Contains(options.ResponsibleContractor.ToLower()))||
                               (a.Contractor.ToLower().Contains(options.Contractor.ToLower()))
            );
        

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
