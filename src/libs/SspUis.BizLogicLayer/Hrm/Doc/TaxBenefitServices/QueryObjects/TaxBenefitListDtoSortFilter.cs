using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public static class TaxBenefitListDtoSortFilter
{
    public static IQueryable<TaxBenefitListDto> SortFilter(this IQueryable<TaxBenefitListDto> query
     , TaxBenefitSortFilterOptions options)
    {
        if(options.StatusId.HasValue)
            query.Where(d => d.StatusId == options.StatusId);

        if(options.OrganizationId.HasValue)
            query.Where(d => d.OrganizationId == options.OrganizationId);

        if(options.HasSearch())
            query = query.Where(a =>
                   a.DocNumber.ToLower().Contains(options.Search.ToLower())
                || a.Organization.ToLower().Contains(options.Search.ToLower())
            );
        query.ToList();
        if(options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);
        return query;
    }
}
