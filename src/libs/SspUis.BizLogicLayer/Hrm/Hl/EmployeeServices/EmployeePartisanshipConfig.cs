using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
namespace SspUis.BizLogicLayer.Hrm;

public class EmployeePartisanshipDtoConfig : PerDtoConfig<EmployeePartisanshipDto, EmployeePartisanship>
{
    public override Action<IMappingExpression<EmployeePartisanship, EmployeePartisanshipDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(e=>e.Partisanship,c=>c.MapFrom(d=>d.Partisanship.FullName));
}
