using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Memship;

public static class DebtListDtoSortFilter
{
    public static IQueryable<DebtListDto> SortFilter(this IQueryable<DebtListDto> query, DebtSortFilterOption options)
    {
        if (options.OrganizationId.HasValue)
            query = query.Where(x => x.OrganizationId == options.OrganizationId);

        if (options.StatusId.HasValue)
            query = query.Where(x => x.StatusId == options.StatusId);

        if (options.HasSearch())
            query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower()) ||
                                     a.TotalDebtAmount.ToString().ToLower().Contains(options.Search.ToLower()) ||
                                     a.TotalEntitlementAmount.ToString().ToLower().Contains(options.Search.ToLower()) ||
                                     a.Organization.ToString().ToLower().Contains(options.Search.ToLower()) ||
                                     a.Id.ToString().ToLower().Contains(options.Search.ToLower())
                                     );

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderBy(a => a.Id);

        return query;
    }
}
