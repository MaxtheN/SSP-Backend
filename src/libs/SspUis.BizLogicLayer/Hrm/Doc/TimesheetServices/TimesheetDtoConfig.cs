using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetDtoConfig : PerDtoConfig<TimesheetDto, Timesheet>
{
    public override Action<IMappingExpression<Timesheet, TimesheetDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Month, x => x.MapFrom(ent => ent.MonthOn.Month))
            .ForMember(x => x.Year, x => x.MapFrom(ent => ent.MonthOn.Year))
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.TimesheetType, c => c.MapFrom(e => e.TimesheetType.Translates.AsQueryable()
                .FirstOrDefault(TimesheetTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.TimesheetType.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            .ForMember(x => x.Tables, x => x.MapFrom(ent => ent.Tables.Where(a => !a.IsDeleted)));
}
