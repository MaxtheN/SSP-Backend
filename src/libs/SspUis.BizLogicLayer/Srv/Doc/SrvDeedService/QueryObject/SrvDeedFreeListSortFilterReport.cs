using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Srv.Doc.SrvDeedService.QueryObject
{
    public static class SrvDeedFreeListSortFilterReport
    {
        public static IQueryable<SrvApplicationListDto> SortFilter2(this IQueryable<SrvApplicationListDto> query, SrvApplicationSortFilterOptions options)
        {
            //if (options.IsFree.HasValue)
            //{
            //    query = options.IsFree.Value
            //        ? query.Where(x => x.IsFree)
            //        : query.Where(x => !x.IsFree);
            //}

            query = query.Where(a => a.StatusId == 2);
            query = query.Where(a => a.IsFree);
            //if (options.StatusId.HasValue && options.StatusId != 0)
            //    query = query.Where(x => x.Application.StatusId == options.StatusId);

            //if (options.RegionId.HasValue && options.RegionId != 0)
            //    query = query.Where(a => a.Application.RegionId == options.RegionId.Value);

            if (options.FromDocDate.HasValue)
                query = query.Where(a => a.Application.DocOn >= options.FromDocDate.Value);

            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.Application.DocOn <= options.ToDocDate.Value);

            //if (options.DistrictId.HasValue && options.DistrictId != 0)
            //    query = query.Where(a => a.Application.DistrictId == options.DistrictId.Value);

            //if (!string.IsNullOrEmpty(options.ContractorInn))
            //    query = query.Where(a => a.Application.ContractorInn == options.ContractorInn);

            //if (options.HasSearch())
            //    query = query.Where(a => a.Application.DocNumber.ToLower().Contains(options.Search.ToLower())
            //                        || a.Application.Contractor.ToLower().Contains(options.Search.ToLower())
            //                        || a.Application.ContractorInn.ToLower().Contains(options.Search.ToLower())
            //                        || a.Application.ContractorPhoneNumber.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
