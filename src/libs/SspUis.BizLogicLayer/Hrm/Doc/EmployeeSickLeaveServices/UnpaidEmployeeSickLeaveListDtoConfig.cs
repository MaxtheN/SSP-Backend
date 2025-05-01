using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UnpaidEmployeeSickLeaveListDtoConfig : PerDtoConfig<UnpaidEmployeeSickLeaveListDto, EmployeeSickLeaveTable>
    {
        public override Action<IMappingExpression<EmployeeSickLeaveTable, UnpaidEmployeeSickLeaveListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.DocumentId, x => x.MapFrom(ent => ent.OwnerId))
            .ForMember(x => x.DocumentStatusId, x => x.MapFrom(ent => ent.Owner.StatusId))
            .ForMember(x => x.DocOn, x => x.MapFrom(ent => ent.Owner.DocOn))
            .ForMember(x => x.DepartmentName, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.EmployeeName, x => x.MapFrom(ent => ent.Employee.Person.FullName))
            .ForMember(x => x.CalculationKindId, x => x.MapFrom(ent => CalculationKindIdConst.SickLeave))
        ;
    }
}
