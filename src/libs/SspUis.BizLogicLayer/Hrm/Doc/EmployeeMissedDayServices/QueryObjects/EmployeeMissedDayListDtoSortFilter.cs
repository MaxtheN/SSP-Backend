using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class EmployeeMissedDayListDtoSortFilter
{
    public static IQueryable<EmployeeMissedDayListDto> SortFilter(this IQueryable<EmployeeMissedDayListDto> query, ISortFilterOptions options)
    {
        if (options is EmployeeMissedDaySortFilterOptions)
        {
            var _options = (EmployeeMissedDaySortFilterOptions)options;

            if (_options.FromDocDate != null && _options.ToDocDate != null)
                query = query.Where(
                    a => a.DocDate >= _options.FromDocDate.Value
                    && a.DocDate <= _options.ToDocDate.Value
                );

        }
        if (options.HasSearch())
            query = query.Where(a => a.Id.ToString().ToLower().Contains(options.Search.ToLower()) ||
              a.EmployeeFullNames.Any(fullName => fullName.ToLower().Contains(options.Search)));
        if (options.HasSort())
            query = query.OrderBy(options.SortBy, options.OrderType);
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
