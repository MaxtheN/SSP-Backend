using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class RecallLeaveTableDtoConfig : PerDtoConfig<RecallLeaveTableDto, RecallLeaveTable>
{
    public override Action<IMappingExpression<RecallLeaveTable, RecallLeaveTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(d => d.Department.FullName))
            .ForMember(d => d.EmployeeId, c => c.MapFrom(d => d.EmployeeId))
            .ForMember(d => d.Employee, c => c.MapFrom(d => d.Employee.Person.FullName));
}
