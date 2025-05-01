using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationEmployeeDtoConfig : PerDtoConfig<JoinAntiCorruptionApplicationEmployeeDto, JoinAntiCorruptionApplicationEmployee>
    {
        public override Action<IMappingExpression<JoinAntiCorruptionApplicationEmployee, JoinAntiCorruptionApplicationEmployeeDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Person, x => x.MapFrom(ent => ent.Person))
                .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position));
    }
}
