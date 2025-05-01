using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSickLeaveTableDtoConfig : PerDtoConfig<EmployeeSickLeaveTableDto, EmployeeSickLeaveTable>
{
    public override Action<IMappingExpression<EmployeeSickLeaveTable, EmployeeSickLeaveTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.DepartmentId, c => c.MapFrom(e => e.DepartmentId))
            .ForMember(d => d.EmployeeId, c => c.MapFrom(e => e.EmployeeId))
             .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName));
}
