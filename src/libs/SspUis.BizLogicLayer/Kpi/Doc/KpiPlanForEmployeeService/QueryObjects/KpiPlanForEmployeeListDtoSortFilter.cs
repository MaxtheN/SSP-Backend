
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Kpi;

public static class KpiPlanForEmployeeListDtoSortFilter
{
    public static IQueryable<KpiPlanForEmployeeListDto> SortFilter(this IQueryable<KpiPlanForEmployeeListDto> query
        , KpiPlanForEmployeeSortFilterOptions options)
    {
        if (options.StatusId.HasValue)
            query.Where(a => a.StatusId == options.StatusId);

        if (options.OrganizationId.HasValue)
            query.Where(a => a.OrganizationId == options.OrganizationId);



        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()));


        //if (options.HasSort())
        //    query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        //else
           query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
