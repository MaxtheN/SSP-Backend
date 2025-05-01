using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class EmployeeLeaveOrderListDtoSortFilter
{
    public static IQueryable<EmployeeLeaveOrderListDto> SortFilter(this IQueryable<EmployeeLeaveOrderListDto> query
    , EmployeeLeaveOrderSortFilterOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()));

        if (options.EmployeeSickLeaveTypeId.HasValue)
        {
            query = query.Where(a => a.EmployeeSickLeaveTypeId == options.EmployeeSickLeaveTypeId);
        }
        else if(options.EmployeeSickLeaveTypeId == null)
        {
            query = query.Where(a => a.EmployeeSickLeaveTypeId != 1 && a.EmployeeSickLeaveTypeId != 2);
        }
        if (options.StatusId.HasValue)
            query = query.Where(a => a.StatusId == options.StatusId);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
    public static IQueryable<UnpaidEmployeeLeaveOrderListDto> SortFilter(this IQueryable<UnpaidEmployeeLeaveOrderListDto> query, UnpaidEmployeeLeaveOrderSortFilterPageOptions options)
    {
        if (options.StartDate.HasValue)
            query = query.Where(a => a.DocOn >= options.StartDate);

        if (options.EndDate.HasValue)
            query = query.Where(a => a.DocOn <= options.EndDate);

        if (options.HasSearch())
            query = query.Where(a => ("" + a.Employee.ToLower()).Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
