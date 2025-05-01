using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Integration.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Administration.ContractorServices;
public static class GetPayDocsSortFilter
{
    public static IQueryable<GetPayDocsDto> SortFilter(this IQueryable<GetPayDocsDto> query, GetPayDocsSortFilterOptions options)
    {
        
            
           

        //else
        //    query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
