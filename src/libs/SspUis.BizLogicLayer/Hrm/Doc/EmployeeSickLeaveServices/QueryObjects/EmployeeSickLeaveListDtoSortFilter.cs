using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class EmployeeSickLeaveListDtoSortFilter
{
    public static IQueryable<EmployeeSickLeaveListDto> SortFilter(this IQueryable<EmployeeSickLeaveListDto> query, EmployeeLeaveOrderSortFilter options)
    {
        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()));

        if (options.EmployeeSickLeaveTypeId.HasValue)
            query = query.Where(a => a.EmployeeSickLeaveTypeId == options.EmployeeSickLeaveTypeId);

        if (options.StatusId.HasValue)
            query = query.Where(a => a.StatusId == options.StatusId);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }

    public static IQueryable<UnpaidEmployeeSickLeaveListDto> SortFilter(this IQueryable<UnpaidEmployeeSickLeaveListDto> query, UnpaidEmployeeSickLeaveSortFilterPageOptions options)
    {
        if (options.StartDate.HasValue)
            query = query.Where(a => a.DocOn.AsDateTime() >= options.StartDate);

        if (options.EndDate.HasValue)
            query = query.Where(a => a.DocOn.AsDateTime() <= options.EndDate);

        if (options.HasSearch())
            query = query.Where(a => ("" + a.EmployeeName.ToLower()).Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
