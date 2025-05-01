using WEBASE.Models;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public static class ContractorUnionActivityTypeListDtoSortFilter
    {
        public static IQueryable<ContractorUnionActivityTypeListDto> SortFilter(this IQueryable<ContractorUnionActivityTypeListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Code.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
