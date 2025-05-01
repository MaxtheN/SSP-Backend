using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.Core;
using SspUis.Core.Security;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingListDtoConfig : PerDtoConfig<StaffingListDto, Staffing>
    {
        public override Action<IMappingExpression<Staffing, StaffingListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.StaffingType, x => x.MapFrom(ent => ent.StaffingType.Translates.AsQueryable().FirstOrDefault(StaffingTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.StaffingType.FullName))
                .ForMember(x => x.SettlementAccountSource, x => x.MapFrom(ent => ent.SettlementAccountSource != null ? ent.SettlementAccountSource.Code : null))
                .ForMember(x => x.OrgSettlementAccount, x => x.MapFrom(ent => ent.OrgSettlementAccount != null ? ent.OrgSettlementAccount.AccountCode : null))
             .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization != null ? ent.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName : ""))



			 .ForMember(x => x.CanSend, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingSend) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.SENT, null)
                        : false))
              .ForMember(x => x.CanReceived, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingReceieved) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.RECEIVED, null)
                        : false))
             .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingReject) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.REJECTED, null)
                        : false))
               .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingCancel) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CANCELED, null)
                        : false))
              .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingAccept) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ACCEPTED, null)
                        : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingEdit) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED, null)
                        : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingDelete) != null
                        ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.DELETED, null)
                        : false))
            .ForMember(x => x.CanClone, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingClone) != null ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.CREATED, null) : false))
            .ForMember(x => x.CanArchive, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.StaffingClone) != null ? StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.ARCHIVED, null) : false))
            ;
    }
}
