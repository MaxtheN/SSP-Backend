using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidEmployeeSendTrainListDtoConfig : PerDtoConfig<UnpaidEmployeeSendTrainListDto, EmployeeSendTrainTable>
    {
        public override Action<IMappingExpression<EmployeeSendTrainTable, UnpaidEmployeeSendTrainListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.DocumentId, x => x.MapFrom(ent => ent.OwnerId))
            .ForMember(x => x.DocumentStatusId, x => x.MapFrom(ent => ent.Owner.StatusId))
            .ForMember(x => x.DocOn, x => x.MapFrom(ent => ent.Owner.DocOn))
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
        ;
    }
}
