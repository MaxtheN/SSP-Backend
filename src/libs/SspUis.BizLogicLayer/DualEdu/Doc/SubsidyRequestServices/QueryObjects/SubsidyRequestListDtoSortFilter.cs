using Microsoft.IdentityModel.Tokens;
using OpenXmlPowerTools;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public static class SubsidyRequestListDtoSortFilter
{
    public static IQueryable<SubsidyRequestListDto> SortFilter(this IQueryable<SubsidyRequestListDto> query, SubsidyRequestSortFilterOption options)
    {
        if (options.OrganizationId.HasValue)
            query = query.Where(x => x.OrganizationId == options.OrganizationId);
        if (options.StatusId.HasValue)
            query = query.Where(x => x.StatusId == options.StatusId);
        if (options.RegionId.HasValue)
            query = query.Where(x => x.RegionId == options.RegionId);
        if (options.DistrictId.HasValue)
            query = query.Where(x => x.DistrictId == options.DistrictId);
        if (!options.ContractorInn.IsNullOrEmpty())
            query = query.Where(x => x.ContractorInnPinfl == options.ContractorInn);
        if (options.FromDocDate.HasValue)
            query = query.Where(x => x.DocOn >= options.FromDocDate);
        if (options.ToDocDate.HasValue)
            query = query.Where(x => x.DocOn <= options.ToDocDate);
       
        if (options.HasSearch())
            query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower()) ||
                                     a.TotalSubsidyAmount.ToString().Contains(options.Search.ToLower()) ||
                                     a.Organization.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Id.ToString().Contains(options.Search.ToLower()) ||
                                     a.Address.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Phone.Replace(" ","").Contains(options.Search.Replace(" ",""))
                                     );

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderBy(a => a.Id);

        return query;
    }
}
