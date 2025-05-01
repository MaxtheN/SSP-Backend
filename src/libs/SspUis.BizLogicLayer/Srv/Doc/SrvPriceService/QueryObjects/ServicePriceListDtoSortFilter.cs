using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer
{
    public static class ServicePriceListDtoSortFilter
    {
        public static IQueryable<ServicePriceListDto> SortFilter(
            this IQueryable<ServicePriceListDto> query,
            ServicePriceSortFilterOption options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower()));

            query = options.HasSort()
                ? query.OrderBy($"{options.SortBy} {options.OrderType}")
                : query.OrderBy(a => a.Id);

            return query;
        }
    }
}
