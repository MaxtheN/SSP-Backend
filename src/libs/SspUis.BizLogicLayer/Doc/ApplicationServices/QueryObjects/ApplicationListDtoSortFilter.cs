using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public static class ApplicationListDtoSortFilter
    {
        public static IQueryable<ApplicationListDto> SortFilter(this IQueryable<ApplicationListDto> query, PrtnDocumentSortFilterOptions options)
        {
            int c = query.Count();
            query = query.DocumentFilter(options);

            if (options.RegionId.HasValue)
                query = query.Where(a => (a.ChooseLocation ? a.ChoosedRegionId : a.ContractorRegionId) == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => (a.ChooseLocation ? a.ChoosedDistrictId : a.ContractorDistrictId) == options.DistrictId.Value);

            if (options.MfyId.HasValue)
                query = query.Where(a => a.MfyId == options.MfyId.Value);

            if (options.PrtnContractTypeId.HasValue)
                query = query.Where(a => a.PrtnContractTypeId == options.PrtnContractTypeId.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => options.ContractorInn == a.ContractorInn);

            if (options.HasSearch())
                query = query.Where(a => a.ApplicationType.ToLower().Contains(options.Search.ToLower())
                                      || a.Contractor.ToLower().Contains(options.Search.ToLower())
                                      || a.ContractorInn.ToLower().Contains(options.Search.ToLower()));

            if (!string.IsNullOrEmpty(options.OkedCode))
                query = query.Where(a => a.OkedCode == options.OkedCode);

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }

        public static IQueryable<ApplicationListDto> SortFilter(this IQueryable<ApplicationListDto> query, StateAssetDocumentSortFilterOptions options)
        {
            query = query.DocumentFilter(options);

            if (options.RegionId.HasValue)
                query = query.Where(a => (a.ChooseLocation ? a.ChoosedRegionId : a.ContractorRegionId) == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => (a.ChooseLocation ? a.ChoosedDistrictId : a.ContractorDistrictId) == options.DistrictId.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => options.ContractorInn == a.ContractorInn);

            if (options.HasSearch())
                query = query.Where(a => a.ApplicationType.ToLower().Contains(options.Search.ToLower())
                                      || a.Contractor.ToLower().Contains(options.Search.ToLower())
                                      || a.ContractorInn.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
