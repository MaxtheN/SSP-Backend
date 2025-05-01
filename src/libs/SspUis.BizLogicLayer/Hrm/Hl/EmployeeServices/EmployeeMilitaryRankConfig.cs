using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeMilitaryRankDtoConfig : PerDtoConfig<EmployeeMilitaryRankDto, EmployeeMilitaryRank>
{
    public override Action<IMappingExpression<EmployeeMilitaryRank, EmployeeMilitaryRankDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.MilitaryRanks, c => c.MapFrom(d => d.MilitaryRanks.FullName));
}
