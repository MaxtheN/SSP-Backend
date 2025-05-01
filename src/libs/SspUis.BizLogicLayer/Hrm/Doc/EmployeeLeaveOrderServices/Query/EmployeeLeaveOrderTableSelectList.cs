using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public static class EmployeeLeaveOrderTableSelectList
    {
        public static SelectList<long> GetTableAsSelectList(this IQueryable<EmployeeLeaveOrderTable> query, long ownerId, long? employeeId)
        {
            query = query.Where(x => x.OwnerId == ownerId);
            if (employeeId.HasValue)
               query = query.Where(a => a.EmployeeId == employeeId);
            
            return new SelectList<long>(
                query
                    .Select(a => new EmployeeLeaveOrderTableDto
                    {
                        Value = a.Id,
                        Text = a.Employee.Person.FullName,
                        Department = a.EmployeeManage.Department.FullName,
                        Position = a.EmployeeManage.Position.FullName,
                        StartOn = a.StartOn,
                        EndOn = a.EndOn,
                        AddPayDays = a.AddPayDays,
                    })
                    .OrderBy(a => a.Text)
                );
        }
        private class EmployeeLeaveOrderTableDto : SelectListItem<long>
        {
            public DateOnly StartOn { get; set; }
            public DateOnly EndOn { get; set; }
            public int AddPayDays { get; set; }
            public string Department { get; set; }
            public string Position{ get; set; }
        }
    }
}
