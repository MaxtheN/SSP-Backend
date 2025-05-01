using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.Core.Security;
using SspUis.Core;
using WEBASE.Security;

namespace SspUis.BizLogicLayer.Hrm;

public class AppointEmployeeListDtoConfig : PerDtoConfig<AppointEmployeeListDto, AppointEmployee>
{
    public override Action<IMappingExpression<AppointEmployee, AppointEmployeeListDto>> AlterReadMapping =>
        cfg => cfg
          .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))
          .ForMember(x => x.OrganizationId, x => x.MapFrom(ent => ent.OrganizationId))
          .ForMember(x => x.Employees, x => x.MapFrom(ent => ent.Tables.Select(a => a.Employee.Person.FullName)))
          //.ForMember(x => x.EmpAppointOrderTypes, x => x.MapFrom(ent => ent.Tables.Select(a => a.EmpAppointOrderType.Translates.AsQueryable().FirstOrDefault(EmpAppointOrderTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? "")))
          .ForMember(x => x.EmployeesId, x => x.MapFrom(ent => ent.Tables.Select(a => a.EmployeeId).ToArray()))
          .ForMember(x => x.EmpAppointOrderTypeId, x => x.MapFrom(ent => ent.Tables.Select(a => a.EmpAppointOrderTypeId).ToArray()))
          .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
          .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppointEmployeeCancel) != null ? StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.NOT_ACCEPTED, null) : false))
          .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppointEmployeeSign) && StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.Signer.Any(signer => signer.EmployeeManageId == ServiceProvider.AuthService.User.EmployeeManageId && (signer.SignFile == null)) ? true : false))
          .ForMember(x => x.CanWithOutSigner, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) && StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
          .ForMember(x => x.CanSignAnyway, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppointEmployeeSignAnyway) && StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.ACCEPTED, null) && ent.Signer.Any(x => x.SignedAt != null)
                            && ent.Signer.OrderByDescending(x => x.SignOrder).FirstOrDefault().EmployeeManageId == ServiceProvider.AuthService.User.EmployeeManageId && ent.StatusId != StatusIdConst.ACCEPTED))
          .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppointEmployeeEdit) != null ? StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.MODIFIED, null) : false))
          .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppointEmployeeDelete) != null ? StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.DELETED, null) : false))
        ;
}