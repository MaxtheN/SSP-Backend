using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSickLeaveListDtoConfig  :PerDtoConfig<EmployeeSickLeaveListDto, EmployeeSickLeave>
{
    public override Action<IMappingExpression<EmployeeSickLeave, EmployeeSickLeaveListDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(x => x.EmployeeNames, x => x.MapFrom(ent => string.Join(", ", ent.Tables.Select(a => a.Employee.Person.FullName))))
        .ForMember(x => x.EmployeeSickLeaveTypeId, x => x.MapFrom(ent => ent.EmployeeSickLeaveTypeId))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
          .ForMember(d => d.EmployeeSickLeaveType, c => c.MapFrom(e => e.EmployeeSickLeaveType.Translates.AsQueryable()
                .FirstOrDefault(EmployeeSickLeaveTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.EmployeeSickLeaveType.FullName))
               .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSickLeaveCancel) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSickLeaveAccept) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSickLeaveEdit) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeSickLeaveDelete) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.DELETED, null)
                        : false));
}
