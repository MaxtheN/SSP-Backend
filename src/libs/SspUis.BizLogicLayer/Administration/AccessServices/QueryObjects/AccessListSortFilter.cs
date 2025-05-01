using SspUis.BizLogicLayer.RoleServices;
using System.Linq;
using WEBASE.Models;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.AccessServices
{
    public static class AccessListSortFilter
    {
        public static IQueryable<AccessListDto> SortFilter(this IQueryable<AccessListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
