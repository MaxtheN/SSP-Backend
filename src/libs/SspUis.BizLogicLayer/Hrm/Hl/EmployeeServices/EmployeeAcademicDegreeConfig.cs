using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeAcademicDegreeDtoConfig : PerDtoConfig<EmployeeAcademicDegreeDto, EmployeeAcademicDegree>
{
    public override Action<IMappingExpression<EmployeeAcademicDegree, EmployeeAcademicDegreeDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.AcademicDegree, c => c.MapFrom(d => d.AcademicDegree.FullName));
}
