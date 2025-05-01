using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Corruption;

public static class JoinAntiCorruptionCertificateListDtoSortFilter
{
    public static IQueryable<JoinAntiCorruptionCertificateListDto> SortFilter(this IQueryable<JoinAntiCorruptionCertificateListDto> query
       , JoinAntiCorruptionCertificateSortFilterOptions options)
    {
        if (options.ContractorId.HasValue && options.ContractorId != 0)
            query = query.Where(x => x.ContractorId == options.ContractorId);

        if (options.RegionId.HasValue)
            query = query.Where(x => x.ApplicationRegionId == options.RegionId);

        if (options.DistrictId.HasValue)
            query = query.Where(x => x.ApplicationDistrictId == options.DistrictId);

        if (options.StatusId.HasValue)
            query = query.Where(x => x.StatusId == options.StatusId);

        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()))
                .AsQueryable();

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
