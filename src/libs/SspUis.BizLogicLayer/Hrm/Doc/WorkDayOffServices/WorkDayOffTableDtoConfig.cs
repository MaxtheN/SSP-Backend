using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class WorkDayOffTableDtoConfig : PerDtoConfig<WorkDayOffTableDto, WorkDayOffTable>
{
    public override Action<IMappingExpression<WorkDayOffTable, WorkDayOffTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
            ;
}
