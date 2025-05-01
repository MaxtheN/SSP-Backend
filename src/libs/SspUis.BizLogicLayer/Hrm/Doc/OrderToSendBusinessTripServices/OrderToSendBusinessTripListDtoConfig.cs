using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class OrderToSendBusinessTripListDtoConfig  :PerDtoConfig<OrderToSendBusinessTripListDto, OrderToSendBusinessTrip>
{
    public override Action<IMappingExpression<OrderToSendBusinessTrip, OrderToSendBusinessTripListDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
                       .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.OrderToSendBusinessTripCancel) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
         .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.OrderToSendBusinessTripSign) && StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null) ? true : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.OrderToSendBusinessTripAccept) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.OrderToSendBusinessTripEdit) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.OrderToSendBusinessTripDelete) != null
                        ? StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, StatusIdConst.DELETED, null)
                        : false));
}
