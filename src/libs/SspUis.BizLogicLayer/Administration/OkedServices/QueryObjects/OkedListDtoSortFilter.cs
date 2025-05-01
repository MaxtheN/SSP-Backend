using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.OkedServices
{
    public static class OkedListDtoSortFilter
    {
        public static IQueryable<OkedListDto> SortFilter(this IQueryable<OkedListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Code.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Level)
                             .ThenBy(a => a.Code);

            return query;
        }

        public static IQueryable<OkedListDto> FilterByParentId(this IQueryable<OkedListDto> query, int? parentId)
        {
            if (parentId.HasValue)
                query = query.Where(a => a.ParentId == parentId);

            return query;
        }
    }
}
