using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetListDtoConfig : PerDtoConfig<TimesheetListDto, Timesheet>
{
    public override Action<IMappingExpression<Timesheet, TimesheetListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.TimesheetType, c => c.MapFrom(e => e.TimesheetType.Translates.AsQueryable()
                .FirstOrDefault(TimesheetTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.TimesheetType.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            .ForMember(x => x.Employees, x => x.MapFrom(ent => string.Join(", ", ent.Tables.Select(a => a.Employee.Person.FullName))))
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(x => x.Month, x => x.MapFrom(ent => ent.MonthOn.Month))
            .ForMember(x => x.Year, x => x.MapFrom(ent => ent.MonthOn.Year))
            .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TimesheetAccept) != null
                        ? StatusIdConst.CanApplyTimesheet(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
            .ForMember(x => x.CanModify, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TimesheetEdit) != null
                        ? StatusIdConst.CanApplyTimesheet(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
            .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TimesheetDelete) != null
                        ? StatusIdConst.CanApplyTimesheet(ent.StatusId, StatusIdConst.DELETED, null)
                        : false))
            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.TimesheetCancel) != null
                        ? StatusIdConst.CanApplyTimesheet(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false));
}
