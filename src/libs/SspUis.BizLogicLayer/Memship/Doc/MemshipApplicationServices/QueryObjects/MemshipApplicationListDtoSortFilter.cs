using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public static class MemshipApplicationListDtoSortFilter
    {
        public static IQueryable<MemshipApplicationListDto> SortFilter(this IQueryable<MemshipApplicationListDto> query, MemshipApplicationSortFilterOptions options)
        {
            //query = query.DocumentFilter(options);

            if (options.StatusId.HasValue && options.StatusId != 0)
                query = query.Where(x => x.Application.StatusId == options.StatusId);

            if (options.ContractorActivityTypeId.HasValue && options.ContractorActivityTypeId != 0)
                query = query.Where(x => x.ContractorActivityTypeId == options.ContractorActivityTypeId);

            if (options.RegionId.HasValue && options.RegionId != 0)
                query = query.Where(a => a.ChooseLocation ? a.ChoosedRegionId == options.RegionId.Value : a.Application.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue && options.DistrictId != 0)
                query = query.Where(a => a.ChooseLocation ? a.ChoosedDistrictId == options.DistrictId.Value : a.Application.DistrictId == options.DistrictId.Value);

            if (options.ContractorCategoryId.HasValue && options.ContractorCategoryId != 0)
                query = query.Where(a => a.ContractorCategoryId == options.ContractorCategoryId.Value);

            if (options.IsPinfl.HasValue)
                query = query.Where(d => options.IsPinfl.Value ? d.Application.ContractorPinfl != null : d.Application.ContractorPinfl == null);

            if (options.FromDocDate.HasValue)
                query = query.Where(a => a.Application.DocOn >= options.FromDocDate.Value);

            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.Application.DocOn <= options.ToDocDate.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => a.Application.ContractorInn == options.ContractorInn);

            if (options.OpfId.HasValue)
                query = query.Where(d => d.OpfId == options.OpfId);

            if (options.HasSearch())
            {
                options.Search = options.Search.ToLower();
                query = query.Where(a => a.Application.DocNumber.ToLower().Contains(options.Search)
                                    || a.Application.Contractor.ToLower().Contains(options.Search)
                                    || a.Application.ContractorPhoneNumber.Contains(options.Search));
            }

            if (options.HasSort())
                //query = query.OrderBy($"{options.SortBy} {options.OrderType}");
                query = query.OrderBy(options.OrderType, options.SortBy);
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
