using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderTableDtoConfig : PerDtoConfig<EmployeeLeaveOrderTableDto, EmployeeLeaveOrderTable>
{
    public override Action<IMappingExpression<EmployeeLeaveOrderTable, EmployeeLeaveOrderTableDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
        .ForMember(x => x.Position, x => x.MapFrom(ent => ent.EmployeeManage.Position.Translates.AsQueryable()
            .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmployeeManage.Position.FullName))
        .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.Employee.Person.FullName))
        .ForMember(x => x.EmployeeManage, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
        .ForMember(x => x.DocDetails, x => x.MapFrom(ent => ent.Details))
        .ForMember(x => x.Days, x => x.MapFrom(ent => ent.Days))
        .ForMember(x => x.TableOrganization, x => x.MapFrom(ent => ent.Owner.Organization.FullName))
        .ForMember(x => x.IsWithOutPay, x => x.MapFrom(ent => ent.IsWithOutPay))
        .ForMember(x => x.IsConscription, x => x.MapFrom(ent => ent.IsConscription))
        .ForMember(x => x.DayOfStartWork, x => x.MapFrom(ent => ent.WorkStartDate));
}
