using System.Linq;
using WEBASE.Models;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.MfyServices
{
    public static class MfyListDtoSortFilter
    {
        public static IQueryable<MfyListDto> SortFilter(this IQueryable<MfyListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Region.ToLower().Contains(options.Search.ToLower()) ||
                                         a.District.ToLower().Contains(options.Search.ToLower())
                                         );

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
