using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses.Public.Hl;
using System;

namespace SspUis.BizLogicLayer;
public class PersonLogDtoCofig : PerDtoConfig<PersonLogDto, PersonLog>
{
    public override Action<IMappingExpression<PersonLog, PersonLogDto>> AlterReadMapping =>
           cfg => cfg
               .ForMember(x => x.Person, x => x.MapFrom(ent => ent.Person.FullName))
           ;
}

