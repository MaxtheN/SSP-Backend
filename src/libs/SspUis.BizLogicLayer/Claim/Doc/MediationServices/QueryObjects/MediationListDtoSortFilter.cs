using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Claim;

public static class MediationListDtoSortFilter
{
    public static IQueryable<MediationListDto> SortFilter(this IQueryable<MediationListDto> query, MediationSortFilterOptions options)
    {
        if (options.StatusId.HasValue)
            query = query.Where(a => a.StatusId == options.StatusId);

        if (options.ContractorId.HasValue)
            query = query.Where(a => a.ContractorId == options.ContractorId);

        if (options.ClaimNeedCourtId.HasValue)
            query = query.Where(a => a.ClaimNeedCourtId == options.ClaimNeedCourtId);

        if (options.MediationResultId.HasValue)
            query = query.Where(a => a.MediationResultId == options.MediationResultId);

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
