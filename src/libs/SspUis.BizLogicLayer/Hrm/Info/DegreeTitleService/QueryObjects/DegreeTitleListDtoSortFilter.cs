using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.Hrm;

public static class DegreeTitleListDtoSortFilter
{
    public static IQueryable<DegreeTitleListDto> SortFilter(this IQueryable<DegreeTitleListDto> query, ISortFilterOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Code.ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
