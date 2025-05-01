using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;



public static class ContractorRatingListDtoSortFilter
{
    public static IQueryable<ContractorRatingListDto> SortFilter(this IQueryable<ContractorRatingListDto> query, ISortFilterOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => 
                                     a.Code.ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}