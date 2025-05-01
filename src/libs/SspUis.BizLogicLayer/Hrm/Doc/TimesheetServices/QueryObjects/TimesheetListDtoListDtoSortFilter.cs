using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public static class TimesheetListDtoListDtoSortFilter
{
    public static IQueryable<TimesheetListDto> SortFilter(this IQueryable<TimesheetListDto> query, TimesheetSortFilterOptions options)
    {
        if (options.DepartmentId.HasValue && options.DepartmentId.Value > 0)
            query = query.Where(x => x.DepartmentId == options.DepartmentId);

        if (options.StartOn.HasValue)
            query = query.Where(x => x.DocOn >= options.StartOn);

        if (options.EndOn.HasValue)
            query = query.Where(x => x.DocOn <= options.EndOn);

        if (options.MonthOn.HasValue)
            query = query.Where(x => x.MonthOn == options.MonthOn);

        if (options.StatusId.HasValue)
            query = query.Where(x => x.StatusId == options.StatusId);

        if (options.TimesheetTypeId.HasValue)
            query = query.Where(x => x.TimesheetTypeId == options.TimesheetTypeId);

        if (options.Id.HasValue && options.Id.Value > 0)
            query = query.Where(x => x.Id == options.Id);

        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()) ||
                                     a.TimesheetType.Contains(options.Search.ToLower()) ||
                                     a.Department.Contains(options.Search.ToLower()) ||
                                     a.DocNumber.Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
