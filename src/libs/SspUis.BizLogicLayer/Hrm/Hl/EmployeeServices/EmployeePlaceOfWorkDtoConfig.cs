using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
namespace SspUis.BizLogicLayer.Hrm;

public class EmployeePlaceOfWorkDtoConfig : PerDtoConfig<EmployeePlaceOfWorkDto, EmployeePlaceOfWork>
{
    public override Action<IMappingExpression<EmployeePlaceOfWork, EmployeePlaceOfWorkDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(e=>e.EmploymentType,c=>c.MapFrom(d=>d.EmploymentType.FullName))
            .ForMember(e=>e.Contractor,c=>c.MapFrom(d=>d.Contractor.FullName));
}
