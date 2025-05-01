using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementTableDtoConfig : PerDtoConfig<ChastisementTableDto, ChastisementTable>
{
    public override Action<IMappingExpression<ChastisementTable, ChastisementTableDto>> AlterReadMapping =>
        cfg => cfg
           .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.FullName))
		.ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName));
}
