using System;
using AutoMapper;
using GenericServices.Configuration;
using OpenXmlPowerTools;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendStudyTableDtoConfig : PerDtoConfig<EmployeeSendStudyTableDto, EmployeeSendStudyTable>
{
    public override Action<IMappingExpression<EmployeeSendStudyTable, EmployeeSendStudyTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName))
            .ForMember(d => d.Position, c => c.MapFrom(e => e.Employee.EmployeeManage.Position.FullName))
            .ForMember(d => d.DocDetails, c => c.MapFrom(e => e.Details))
        ;
}
