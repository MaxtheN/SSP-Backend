using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class AppointEmployeeDtoConfig : PerDtoConfig<AppointEmployeeDto, AppointEmployee>
{
    public override Action<IMappingExpression<AppointEmployee, AppointEmployeeDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(d => d.Region, c => c.MapFrom(e => e.Organization.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, 3)).TranslateText ?? e.Organization.Region.FullName))
            .ForMember(d => d.Employees, c => c.MapFrom(ent => ent.Tables.Select(x => x.Employee.Person)))
            .ForMember(d => d.Details, c => c.MapFrom(ent => ent.Details))
            .ForMember(d => d.ConclusionForPrint, c => c.MapFrom(ent => ent.ConclusionForPrint))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
        ;
}