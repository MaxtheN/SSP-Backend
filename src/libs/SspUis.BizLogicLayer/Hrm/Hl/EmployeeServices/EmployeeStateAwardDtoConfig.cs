using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeStateAwardDtoConfig : PerDtoConfig<EmployeeStateAwardDto, EmployeeStateAward>
{
    public override Action<IMappingExpression<EmployeeStateAward, EmployeeStateAwardDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d=>d.StateAwards, c=>c.MapFrom(d=>d.StateAwards.FullName))
        ;
}
