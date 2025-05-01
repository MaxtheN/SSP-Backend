using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class InstituteBillingListDtoSortFilter
{
    public static IQueryable<InstituteBillingListDto> SortFilter(this IQueryable<InstituteBillingListDto> query, ISortFilterOptions options)
    {
        if(options.HasSearch())
            query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.OrderCode.ToLower().Contains(options.Search.ToLower()));

        if(options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderBy(a => a.Id);

        return query;
    }
}