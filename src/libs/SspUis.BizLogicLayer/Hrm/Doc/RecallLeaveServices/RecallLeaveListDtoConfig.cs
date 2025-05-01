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

public class RecallLeaveListDtoConfig : PerDtoConfig<RecallLeaveListDto, RecallLeave>
{
    public override Action<IMappingExpression<RecallLeave, RecallLeaveListDto>> AlterReadMapping =>
        cfg=>cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
                       .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.RecallLeaveCancel) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
        .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.RecallLeaveSign) && StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null) ? true : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.RecallLeaveAccept) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.RecallLeaveEdit) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.RecallLeaveDelete) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.DELETED, null)
                        : false));
}
