using WEBASE.Models;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;

public static class AdditionalAgreementListDtoSortFilter
{
    public static IQueryable<AdditionalAgreementListDto> SortFilter(this IQueryable<AdditionalAgreementListDto> query
         , AdditionalAgreementSortFilterOption options)
    {
        if (options.HasSearch())
            query = query.Where(a => a.Status.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Organization.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Contractor.ToLower().Contains(options.Search.ToLower()) ||
                                     a.ContractorInn.ToLower().Contains(options.Search.ToLower()) ||
                                     a.ApplicationType.ToLower().Contains(options.Search.ToLower()) ||
                                     a.DocNumber.ToLower().Contains(options.Search.ToLower()));

        if (options.RegionId.HasValue)
            query = query.Where(d => d.RegionId == options.RegionId.Value);

        if (options.FromDocDate.HasValue)
            query = query.Where(d => d.DocOn >= options.FromDocDate);

        if (options.ToDocDate.HasValue)
            query = query.Where(d => d.DocOn <= options.ToDocDate);

        if (!options.ContractorInn.IsNullOrEmpty())
            query = query.Where(d => d.ContractorInn == options.ContractorInn);

        if (options.DistrictId.HasValue)
            query = query.Where(d => d.DistrictId == options.DistrictId);
        if (options.StatusId.HasValue)
            query = query.Where(d => d.StatusId == options.StatusId);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
