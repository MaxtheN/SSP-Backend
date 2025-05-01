using System.Linq;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
namespace SspUis.BizLogicLayer.Hrm;

public static class DepartmentDtoSortFilter
{
    public static IQueryable<DepartmentListDto> SortFilter(this IQueryable<DepartmentListDto> query
        ,ISortFilterOptions options)
    {
        if(options.HasSearch())
            query = query.Where(a => a.FullName.ToLower().Contains(options.Search.ToLower())
                || a.FullName.ToLower().Contains(options.Search.ToLower())
                || a.ShortName.ToLower().Contains(options.Search.ToLower())
            );
        query.ToList();
        if(options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderBy(a => a.Code).ThenBy(a => a.OrderCode);
        return query;
    }
}
