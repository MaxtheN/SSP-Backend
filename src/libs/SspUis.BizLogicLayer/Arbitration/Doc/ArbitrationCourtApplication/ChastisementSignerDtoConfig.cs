using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class ArbitrationCourtApplicationSignDtoConfig : PerDtoConfig<ArbitrationCourtApplicationSignDto, ArbitrationCourtApplicationSigner>
{
    public override Action<IMappingExpression<ArbitrationCourtApplicationSigner, ArbitrationCourtApplicationSignDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.FullName))
            .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
            .ForMember(x => x.ArbitrationJudge, x => x.MapFrom(ent => ent.ArbitrationJudge.FirstName))
            .ForMember(x => x.DocNumber, x => x.MapFrom(ent => ent.Owner.Application.DocNumber))
            .ForMember(x => x.DocOn, x => x.MapFrom(ent => ent.Owner.Application.DocOn))
        ;

}
