using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public static class EmployeeManageListDtoSortFilter
    {
        public static IQueryable<EmployeeManageListDto> SortFilter(this IQueryable<EmployeeManageListDto> query, EmployeeManageSortFilterPageOptions options)
        {
            if (options.IsOnlyWorkingEmployee)
                query = query.Where(a => a.EndOn == null);
            else if (!options.IsOnlyWorkingEmployee)
                query = query.Where(a => a.EndOn != null);
            if (options.EmployeeId.HasValue)
                query = query.Where(a => a.EmployeeId == options.EmployeeId.Value);
            if (options.DepartmentId.HasValue)
                query = query.Where(a => a.DepartmentId == options.DepartmentId.Value);
            if (options.PositionId.HasValue)
                query = query.Where(a => a.PositionId == options.PositionId.Value);
            if (options.OrganizationId.HasValue && options.OrganizationId.Value > 0)
                query = query.Where(a => a.OrganizationId == options.OrganizationId.Value);

            if (options.HasSearch())
                query = query.Where(a =>
                    a.Employee.ToLower().Contains(options.Search.ToLower())
                    || a.EmployeePinfl.ToLower().Contains(options.Search.ToLower())
                    || a.EmployeePhoneNumber.ToLower().Contains(options.Search.ToLower())
                    || a.DepartmentId.ToString().ToLower().Contains(options.Search.ToLower())
                );

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
