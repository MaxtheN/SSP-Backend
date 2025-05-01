using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeDegreeTitleConfig : PerDtoConfig<EmployeeDegreeTitleDto, EmployeeDegreeTitle>
{
    public override Action<IMappingExpression<EmployeeDegreeTitle, EmployeeDegreeTitleDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(e=>e.DegreeTitle,c=>c.MapFrom(d=>d.DegreeTitle.FullName));
}
