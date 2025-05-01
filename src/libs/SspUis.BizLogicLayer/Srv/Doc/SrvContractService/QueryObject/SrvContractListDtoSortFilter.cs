using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer
{
    public static class SrvContractListDtoSortFilter
    {
        public static IQueryable<SrvContractListDto> SortFilter(this IQueryable<SrvContractListDto> query
        , SrvContractSortFilterOption options)
        {
            if (options.HasSearch())
                query = query.Where(a => (a.DocNumber).ToLower().Contains(options.Search.ToLower()) ||
                                          a.ContractorInn.ToLower().Contains(options.Search.ToLower()) ||
                                          a.Organization.ToLower().Contains(options.Search.ToLower()));

            if (options.StatusId.HasValue && options.StatusId != 0)
                query = query.Where(d => d.StatusId == options.StatusId);

            if (options.OrganizationId.HasValue)
                query = query.Where(d => d.OrganizationId == options.OrganizationId);

            if (!options.ContractorInn.IsNullOrEmpty())
                query = query.Where(d => d.ContractorInn == options.ContractorInn);

            if (options.FromDocDate.HasValue)
                query = query.Where(d => d.DocOn >= options.FromDocDate);

            if (options.ToDocDate.HasValue)
                query = query.Where(d => d.DocOn <= options.ToDocDate);

            if (options.ContractorRegionId.HasValue)
                query = query.Where(a => a.ContractorRegionId == options.ContractorRegionId);

            if (options.ContractorDistrictId.HasValue)
                query = query.Where(a => a.ContractorDistrictId == options.ContractorDistrictId);

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}