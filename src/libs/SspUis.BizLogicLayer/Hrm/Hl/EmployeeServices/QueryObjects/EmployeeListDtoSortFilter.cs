using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public static class EmployeeListDtoSortFilter
{
    public static IQueryable<EmployeeListDto> SortFilter(this IQueryable<EmployeeListDto> query, EmployeeSortFilterPageOptions options)
    {
        if (options.HasSearch())
            query = query.Where(a => a.FullName.ToLower().Contains(options.Search.ToLower()));

        if (options.OrganizationId.HasValue && options.OrganizationId != 0)
            query = query.Where(x => x.OrganizationId == options.OrganizationId);

        if (options.RegionId.HasValue)
            query = query.Where(a => a.RegionId == options.RegionId);

        if (options.MinAge.HasValue)
        {
            var minDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-options.MinAge.Value);
            query = query.Where(a => a.BirthDate <= minDate);
        }

        if (options.MaxAge.HasValue)
        {
            var maxDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-options.MaxAge.Value - 1);
            query = query.Where(a => a.BirthDate > maxDate);
        }

        if (options.DistrictId.HasValue)
            query = query.Where(a => a.DistrictId == options.DistrictId);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }

    public static IQueryable<EmployeeListDto> FilterByCustomFields(this IQueryable<EmployeeListDto> query, EmployeeSortFilterPageOptions options)
    {
        if (options.OrganizationId.HasValue && options.ParentOrganizationId.Value != options.OrganizationId.Value)
            query = query.Where(a => a.OrganizationId == options.OrganizationId);

        return query;
    }
}
