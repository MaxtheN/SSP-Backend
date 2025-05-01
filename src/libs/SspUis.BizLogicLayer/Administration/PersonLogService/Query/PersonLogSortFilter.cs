using SspUis.BizLogicLayer.Appeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.PersonLogService.Query
{
    public static class PersonLogSortFilter
    {
        public static IQueryable<PersonLogListDto> SortFilter(this IQueryable<PersonLogListDto> query
                 , PersonLogSortFilterOption options)
        {
           

            if (options.EmployeeId != null)
                query = query.Where(d => d.EmployeeId.Equals(options.EmployeeId));



            if (options.HasSort())
                query = query.OrderByDescending(a => a.Id);
            return query;
        }
    }
}
