using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderListDtoConfig : PerDtoConfig<EmployeeLeaveOrderListDto, EmployeeLeaveOrder>
{
    public override Action<IMappingExpression<EmployeeLeaveOrder, EmployeeLeaveOrderListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
			.ForMember(x => x.Employee, x => x.MapFrom(ent => string.Join(", ", ent.Tables.Select(a => a.Employee.Person.FullName))))
            //.ForMember(x => x.OrgActivityTypeName, x => x.MapFrom(ent => ent.OrganizationActivityType.Translates.AsQueryable().FirstOrDefault(OrganizationActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.OrganizationActivityType.FullName))
            .ForMember(x => x.IsWithOutPay, x => x.MapFrom(ent => ent.Tables.FirstOrDefault() != null ? ent.Tables.FirstOrDefault().IsWithOutPay : false))
               .ForMember(x => x.IsWithOutPay, x => x.MapFrom(ent => ent.Tables.FirstOrDefault() != null ? ent.Tables.FirstOrDefault().IsWithOutPay : false))
        .ForMember(d => d.EmployeeSickLeaveType, c => c.MapFrom(e => e.EmployeeSickLeaveType.Translates.AsQueryable()
                .FirstOrDefault(EmployeeSickLeaveTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.EmployeeSickLeaveType.FullName))
               .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeLeaveOrderCancel) != null ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.NOT_ACCEPTED, null) : false))
               .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeLeaveOrderSign) && StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeLeaveOrderAccept) && StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null) ? true : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeLeaveOrderEdit) != null ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.MODIFIED, null) : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.EmployeeLeaveOrderDelete) != null ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.DELETED, null) : false));
}