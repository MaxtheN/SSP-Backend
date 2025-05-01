using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public static class DualApplicationListDtoSortFilter
    {
        public static IQueryable<DualApplicationListDto> SortFilter(this IQueryable<DualApplicationListDto> query, DualApplicationSortFilterOptions options)
        {
            query = query.DocumentFilter(options);

            if(options.RegionId.HasValue)
                query = query.Where(a=>a.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.DistrictId == options.DistrictId.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a=>a.ContractorInn == options.ContractorInn);

            if (options.HasSearch()) 
                query = query.Where(a=>a.DocNumber.ToLower().Contains(options.Search.ToLower()) 
                                    || a.Contractor.ToLower().Contains(options.Search.ToLower())
                                    || a.ContractorInn.ToLower().Contains(options.Search.ToLower())
                                    || a.ContractorPhoneNumber.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
