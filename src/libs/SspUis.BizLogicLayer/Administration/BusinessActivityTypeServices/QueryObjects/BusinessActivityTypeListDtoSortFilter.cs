using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public static class BusinessActivityTypeListDtoSortFilter
    {
        public static IQueryable<BusinessActivityTypeListDto> SortFilter(this IQueryable<BusinessActivityTypeListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.Contractor.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FinancialHelp.ToLower().Contains(options.Search.ToLower()) ||
                                         a.BusinessCtorName.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
