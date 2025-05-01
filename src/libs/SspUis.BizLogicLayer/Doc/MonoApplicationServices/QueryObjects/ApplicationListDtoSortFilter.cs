using SspUis.Core;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public static class MonoApplicationListDtoSortFilter
    {
        public static IQueryable<MonoApplicationListDto> SortFilter(this IQueryable<MonoApplicationListDto> query, MonoApplicationSortFilterOptions options)
        {
           query = query.Where(a => a.StatusId != StatusIdConst.DELETED);
            if (options.StatusId.HasValue && options.StatusId != 0)
                query = query.Where(x => x.Application.StatusId == options.StatusId);

            if (options.RegionId.HasValue)
                query = query.Where(a=>a.Application.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.Application.DistrictId == options.DistrictId.Value);

            if (options.MonoDistrictId.HasValue)
                query = query.Where(a => a.MonoDistrictId == options.MonoDistrictId.Value);

            if (options.MonoRegionId.HasValue)
                query = query.Where(a => a.MonoRegionId == options.MonoRegionId.Value);

            if (options.FromDocDate.HasValue)
                query = query.Where(a => a.DocOn >= options.FromDocDate.Value);
            
            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.DocOn <= options.ToDocDate.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a=>a.Application.ContractorInn == options.ContractorInn);

            if (options.HasSearch()) 
                query = query.Where(a=>a.Application.DocNumber.ToLower().Contains(options.Search.ToLower()) 
                                    || a.Application.Contractor.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.ContractorInn.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.ContractorPhoneNumber.ToLower().Contains(options.Search.ToLower())
                                    );

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
