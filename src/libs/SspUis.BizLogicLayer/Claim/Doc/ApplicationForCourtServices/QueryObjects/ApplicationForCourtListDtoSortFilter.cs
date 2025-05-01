using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer.Claim;

public static class ApplicationForCourtListDtoSortFilter
{
    public static IQueryable<ApplicationForCourtListDto> SortFilter(this IQueryable<ApplicationForCourtListDto> query, ApplicationForCourtSortFilterOptions options)
    {
        if (options.StepId.HasValue && options.StepId != null)
            query = query.Where(a => a.StepId == options.StepId);

        if (!options.ContractorInn.IsNullOrEmpty())
            query = query.Where(a => a.ContractorInn == options.ContractorInn);

        if (options.StatusId.HasValue)
            query = query.Where(a => a.StatusId == options.StatusId);

        if (options.ContractorId.HasValue)
            query = query.Where(a => a.ContractorId == options.ContractorId);

        if (options.ClaimOrganizationId.HasValue)
            query = query.Where(a => a.ClaimOrganizationId == options.ClaimOrganizationId);

        if (options.FromDocDate.HasValue)
            query = query.Where(a => a.DocOn >= options.FromDocDate.Value);

        if (options.ToDocDate.HasValue)
            query = query.Where(a => a.DocOn <= options.ToDocDate.Value);

        if (options.HasSearch())
            query = query.Where(a => a.Id.ToString().ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
