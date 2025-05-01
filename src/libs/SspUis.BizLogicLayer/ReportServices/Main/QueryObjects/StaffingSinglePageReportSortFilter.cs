using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ReportServices
{
    public static class StaffingSinglePageReportSortFilter
    {
        public static IQueryable<StaffingSinglePageReportDto> SortFilter(this IQueryable<StaffingSinglePageReportDto> query, StaffingSinglePageReportDtoFilter options)
        {

            //if (options.Acting)
            //    query = query.Where(a => a.EmployeeManageTables.Any(b => b.AppointEmployees.Any(s => s.Acting && !s.Interm)));
            if (options.Acting)
            {
                // Materialize the data for the EmployeeManageTables and AppointEmployees
                var employeeManageTables = query.SelectMany(a => a.EmployeeManageTables).ToList();

                // Filter the EmployeeManageTables based on IsProbation
                var filteredEmployeeManageTables = employeeManageTables
                    .Where(b => b.AppointEmployees.Any(s => s.Acting))
                    .Select(b => b.EmployeeManageId)
                    .ToList();

                // Filter the main query based on the filteredEmployeeManageTables
                query = query.Where(a => a.EmployeeManageTables.Any(b => filteredEmployeeManageTables.Contains(b.EmployeeManageId)));
            }

            //if (options.Interm)
            //    query = query.Where(a => a.EmployeeManageTables.Any(b => b.AppointEmployees.Any(s => s.Interm && !s.Acting)));

            if (options.Interm)
            {
                // Materialize the data for the EmployeeManageTables and AppointEmployees
                var employeeManageTables = query.SelectMany(a => a.EmployeeManageTables).ToList();

                // Filter the EmployeeManageTables based on IsProbation
                var filteredEmployeeManageTables = employeeManageTables
                    .Where(b => b.AppointEmployees.Any(s => s.Interm))
                    .Select(b => b.EmployeeManageId)
                    .ToList();

                // Filter the main query based on the filteredEmployeeManageTables
                query = query.Where(a => a.EmployeeManageTables.Any(b => filteredEmployeeManageTables.Contains(b.EmployeeManageId)));
            }

            //if (options.Employees != null)
            //    query = query.Where(a => a.EmployeeManageTables.Any(b => b.Employees == options.Employees));

            //if (options.IsProbation)
            //    query = query.Where(a => a.EmployeeManageTables.Any(b => b.AppointEmployees.Any(s => s.IsProbation)));

            if (options.IsProbation)
            {
                // Materialize the data for the EmployeeManageTables and AppointEmployees
                var employeeManageTables = query.SelectMany(a => a.EmployeeManageTables).ToList();

                // Filter the EmployeeManageTables based on IsProbation
                var filteredEmployeeManageTables = employeeManageTables
                    .Where(b => b.AppointEmployees.Any(s => s.IsProbation))
                    .Select(b => b.EmployeeManageId)
                    .ToList();

                // Filter the main query based on the filteredEmployeeManageTables
                query = query.Where(a => a.EmployeeManageTables.Any(b => filteredEmployeeManageTables.Contains(b.EmployeeManageId)));
            }

            if (options.PositionId.HasValue)
                query = query.Where(a => a.PositionId == options.PositionId);

            if (options.DepartmentId.HasValue)
                query = query.Where(a => a.DepartmentId == options.DepartmentId);

            if (options.ByQuantity)
                query = query.Where(a => a.Quantity == 1 && a.QuantityForNow == 1);

            if (options.ByQuantityForNow)
                query = query.Where(a => a.Quantity == 1 && a.QuantityForNow == 0);

            if (options.StartDate.HasValue)
                query = query.Where(a => a.EmployeeManageTables.Any(a => a.AppointEmployees.Any(s => s.DocOn >= options.StartDate)));

            if (options.EndDate.HasValue)
                query = query.Where(a => a.EmployeeManageTables.Any(a => a.AppointEmployees.Any(s => s.DocOn <= options.EndDate)));

            if (options.StartCreatedAt.HasValue)
                query = query.Where(a => a.EmployeeManageTables.Any(a => a.AppointEmployees.Any(s => s.CreatedAt >= options.StartCreatedAt)));

            if (options.EndCreatedAt.HasValue)
                query = query.Where(a => a.EmployeeManageTables.Any(a => a.AppointEmployees.Any(s => s.CreatedAt <= options.EndCreatedAt)));

            if (options.HasSearch())
                query = query.Where(a =>
                                         a.Department.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Position.ToLower().Contains(options.Search.ToLower()) ||
                                         a.EmployeeManageTables.Any(x => x.Employees.ToLower().Contains(options.Search.ToLower())));


            return query;
        }
    }
}