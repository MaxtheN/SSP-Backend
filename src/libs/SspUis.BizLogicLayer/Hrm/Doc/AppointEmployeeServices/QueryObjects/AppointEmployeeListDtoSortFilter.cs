using OpenXmlPowerTools;
using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class AppointEmployeeListDtoSortFilter
{
    public static IQueryable<AppointEmployeeListDto> SortFilter(this IQueryable<AppointEmployeeListDto> query
       , ISortFilterOptions options)
    {
        if (options is AppointEmployeeSortFilterOptions dto)
        {
            query = query.Where(a => dto.EmployeeId.HasValue ? a.EmployeesId.Any(a => a == dto.EmployeeId.Value) : true &&
                (dto.StatusIds != null && dto.StatusIds.Any() ? dto.StatusIds.Contains(a.StatusId) : true)
            );
            query = query.Where(a => dto.EmpAppointOrderTypeId.HasValue ? a.EmpAppointOrderTypeId.Any(a => a == dto.EmpAppointOrderTypeId.Value) : true &&
               (dto.StatusIds != null && dto.StatusIds.Any() ? dto.StatusIds.Contains(a.StatusId) : true)
           );
            if (dto.StartOn.HasValue)
            {
                query = query.Where(a => a.DocOn >= dto.StartOn);
            }
            if (dto.EndOn.HasValue)
            {
                query = query.Where(a => a.DocOn <= dto.EndOn);
            }
        }
       
        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber.ToLower()).Contains(options.Search.ToLower()) ||
                                    a.Status.ToLower().Contains(options.Search.ToLower()) ||
                                    a.Employees.Any(b => b.ToLower().Contains(options.Search.ToLower())) ||
                                    a.Details.ToLower().Contains(options.Search.ToLower()))
                .AsQueryable();

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
