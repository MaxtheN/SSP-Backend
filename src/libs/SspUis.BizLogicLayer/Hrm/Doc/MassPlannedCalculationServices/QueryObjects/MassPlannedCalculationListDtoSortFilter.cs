using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public static class MassPlannedCalculationListDtoSortFilter
{
    public static IQueryable<MassPlannedCalculationListDto> SortFilter(this IQueryable<MassPlannedCalculationListDto> query
     , MassPlannedCalculationSortFilterOptions options)
    {
        if (options.StatusId.HasValue)
            query.Where(a => a.StatusId == options.StatusId);

        if (options.OrganizationId.HasValue)
            query.Where(a => a.OrganizationId == options.OrganizationId);

        if (options.OrgSettlementAccountId.HasValue)
            query.Where(a => a.OrgSettlementAccountId == options.OrgSettlementAccountId);

        if (options.RoundingTypeId.HasValue)
            query.Where(a => a.RoundingTypeId == options.RoundingTypeId);

        if (options.CalculationKindId.HasValue)
            query.Where(a => a.CalculationKindId == options.CalculationKindId);


        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()) ||
                                      a.CalculationKind.Contains(options.Search.ToLower()) ||
                                      a.Organization.Contains(options.Search.ToLower()) ||
                                      a.OrgSettlementAccountCode.Contains(options.Search.ToLower()) ||
                                      a.RoundingType.Contains(options.Search.ToLower()));
                                      

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
