using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public static class StateAssetApplicationListDtoSortFilter
    {
        public static IQueryable<StateAssetApplicationListDto> SortFilter(this IQueryable<StateAssetApplicationListDto> query, StateAssetDocumentSortFilterOptions options)
        {
            query = query.DocumentFilter(options);

            if (options.RegionId.HasValue)
                query = query.Where(a => a.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.DistrictId == options.DistrictId.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => options.ContractorInn == a.ContractorInn);

            if (options.HasSearch())
                query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower())
                                      || a.Contractor.ToLower().Contains(options.Search.ToLower())
                                      || a.ContractorInn.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
