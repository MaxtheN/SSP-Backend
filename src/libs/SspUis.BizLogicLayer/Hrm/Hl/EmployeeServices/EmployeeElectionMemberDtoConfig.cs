using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeElectionMemberDtoConfig : PerDtoConfig<EmployeeElectionMemberDto, EmployeeElectionMember>
{
    public override Action<IMappingExpression<EmployeeElectionMember, EmployeeElectionMemberDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d=>d.ElectionMember,c=>c.MapFrom(d=>d.ElectionMember.FullName))
        ;
}
