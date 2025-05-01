using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public static class LandingPageDatumListDtoSortFilter
    {
        public static IQueryable<LandingPageDatumListDto> SortFilter(this IQueryable<LandingPageDatumListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.Label.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Value.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
