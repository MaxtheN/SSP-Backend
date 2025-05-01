using WEBASE.Models;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public static class QualificationCategoryListDtoSortFilter
    {
        public static IQueryable<QualificationCategoryListDto> SortFilter(this IQueryable<QualificationCategoryListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Code.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Details.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
