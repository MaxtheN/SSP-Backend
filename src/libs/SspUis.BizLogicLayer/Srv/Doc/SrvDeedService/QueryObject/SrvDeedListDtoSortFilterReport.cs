using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer.Srv.Doc.SrvDeedService.QueryObject
{
    public static class SrvDeedListDtoSortFilterReport
    {
        public static IQueryable<SrvDeedListDto> SortFilterReport(this IQueryable<SrvDeedListDto> query
       , SrvDeedListReportSortFilter options)
        {
            var count = query.Count();
            if (options.HasSearch())
                query = query.Where(a => (a.DocNumber).ToLower().Contains(options.Search.ToLower()) ||
                                          a.ContractorInn.ToLower().Contains(options.Search.ToLower()) ||
                                          a.Organization.ToLower().Contains(options.Search.ToLower()));


            if (options.FromDocDate.HasValue)
                query = query.Where(d => d.DocOn >= options.FromDocDate);

            if (options.ToDocDate.HasValue)
                query = query.Where(d => d.DocOn <= options.ToDocDate);

            count = query.Count();
            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
