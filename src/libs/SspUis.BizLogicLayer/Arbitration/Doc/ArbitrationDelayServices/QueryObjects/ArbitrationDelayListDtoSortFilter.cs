using System.Linq;
using System.Linq.Dynamic.Core;
using SspUis.BizLogicLayer.Arbitration.Doc.ArbitrationDelayServices.QueryObjects;

namespace SspUis.BizLogicLayer;

public static class ArbitrationDelayListDtoSortFilter
{
    public static IQueryable<ArbitrationDelayListDto> SortFilter(
        this IQueryable<ArbitrationDelayListDto> query,
        ArbitrationDelaySortFilterOptions options)
    {
        if (options.FromDocDate.HasValue)
            query = query.Where(a => a.DocOn >= options.FromDocDate);

        if (options.ToDocDate.HasValue)
            query = query.Where(a => a.DocOn <= options.ToDocDate);

        if (options.HasSearch())
            query = query.Where(a =>
                        a.DocNumber.ToLower().Contains(options.Search.ToLower())
                        || a.Organization.ToLower().Contains(options.Search.ToLower())
                        || a.Contractor.ToLower().Contains(options.Search.ToLower())
                        || a.ContractorInn.ToLower().Contains(options.Search.ToLower())
                        || a.ResponsibleContractor.ToLower().Contains(options.Search.ToLower())
                        || a.ResponsibleContractorInn.ToLower().Contains(options.Search.ToLower())
                        );

        if (options.OrganizationId.HasValue)
            query = query.Where(a => a.OrganizationId == options.OrganizationId.Value);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
