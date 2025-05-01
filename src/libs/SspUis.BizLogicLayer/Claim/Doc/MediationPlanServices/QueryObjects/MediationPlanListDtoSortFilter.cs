using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Claim;

public static class MediationPlanListDtoSortFilter
{
    public static IQueryable<MediationPlanListDto> SortFilter(this IQueryable<MediationPlanListDto> query
     , MediationPlanSortFilterOptions options)
    {
        if(options.StatusId.HasValue)
            query = query.Where(d => d.StatusId == options.StatusId);

        if(options.HasSearch())
            query = query.Where(a =>
                   a.DocNumber.ToLower().Contains(options.Search.ToLower())
                || a.MeetingType.ToLower().Contains(options.Search.ToLower())
            );

        if (options.ContractorId.HasValue)
            query = query.Where(a => a.ContractorId == options.ContractorId.Value);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);
        return query;
    }
}
