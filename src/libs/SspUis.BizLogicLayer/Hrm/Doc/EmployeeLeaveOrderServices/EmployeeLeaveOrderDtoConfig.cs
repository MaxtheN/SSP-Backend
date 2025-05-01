using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderDtoConfig : PerDtoConfig<EmployeeLeaveOrderDto, EmployeeLeaveOrder>
{
    public override Action<IMappingExpression<EmployeeLeaveOrder, EmployeeLeaveOrderDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Organization.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.Region.FullName))
        .ForMember(d => d.Employees, c => c.MapFrom(ent => ent.Tables.Select(x => x.Employee.Person)))
        .ForMember(d => d.EmployeeSickLeaveType, c => c.MapFrom(e => e.EmployeeSickLeaveType.Translates.AsQueryable()
                .FirstOrDefault(EmployeeSickLeaveTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.EmployeeSickLeaveType.FullName));
}
