using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidChastisementListDtoConfig : PerDtoConfig<UnpaidChastisementListDto, ChastisementTable>
    {
        public override Action<IMappingExpression<ChastisementTable, UnpaidChastisementListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.DocumentId, x => x.MapFrom(ent => ent.OwnerId))
            .ForMember(x => x.DocumentStatusId, x => x.MapFrom(ent => ent.Owner.StatusId))
            .ForMember(x => x.DocOn, x => x.MapFrom(ent => ent.Owner.DocOn))
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
            //.ForMember(x => x.CalculationKindId, x => x.MapFrom(ent => CalculationKindIdConst.LaborLeave))
        ;
    }
}
