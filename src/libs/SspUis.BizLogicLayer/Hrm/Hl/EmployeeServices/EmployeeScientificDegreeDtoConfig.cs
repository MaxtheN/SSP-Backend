using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeScientificDegreeDtoConfig : PerDtoConfig<EmployeeScientificDegreeDto, EmployeeScientificDegree>
{
    public override Action<IMappingExpression<EmployeeScientificDegree, EmployeeScientificDegreeDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d=>d.ScientificDegree,c=>c.MapFrom(d=>d.ScientificDegree.FullName))
        ;
}
