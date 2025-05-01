using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer;

public class CallCenterAppealListDtoConfig : PerDtoConfig<CallCenterAppealListDto, CallCenterAppeal>
{
    public override Action<IMappingExpression<CallCenterAppeal, CallCenterAppealListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Contractor, c => c.MapFrom(e => e.Contractor.FullName))
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.AppealType, c => c.MapFrom(e => e.AppealType.FullName))
            .ForMember(d => d.PersonName, c => c.MapFrom(e => e.Person.FullName ?? e.PersonFullName ?? e.Contractor.Director))
            .ForMember(d => d.ContractorInn, c => c.MapFrom(e => e.Contractor.Pinfl ?? e.Contractor.Inn))
            .ForMember(d => d.PhoneNumber, c => c.MapFrom(e => e.Phonenumber ?? e.Contractor.PhoneNumber))
            .ForMember(d => d.Pinfl, c => c.MapFrom(e => e.Person.Pinfl))
            .ForMember(d => d.PassportSeria, c => c.MapFrom(e => e.Person.PassportSeria))
            .ForMember(d => d.PassportNumber, c => c.MapFrom(e => e.Person.PassportNumber))
            .ForMember(d => d.AppealFormatType, c => c.MapFrom(e => e.AppealFormatType.FullName))
            .ForMember(d => d.RegionId, c => c.MapFrom(e => e.RegionId))
            .ForMember(d => d.Isexporter, c => c.MapFrom(e => e.Isexporter))
            .ForMember(d => d.Isimporter, c => c.MapFrom(e => e.Isimporter))
            .ForMember(d => d.OkedCode, c => c.MapFrom(e => e.Contractor.Oked.Code))
            .ForMember(d => d.ContractorDirectorName, c => c.MapFrom(e => e.Contractor.Director))
            .ForMember(d => d.Oked, c => c.MapFrom(e => e.Contractor.Oked.Translates.AsQueryable()
                .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Contractor.Oked.FullName))

            .ForMember(d => d.AppealDescription, c => c.MapFrom(e => e.AppealDescription.Translates.AsQueryable()
                .FirstOrDefault(AppealDescriptionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealDescription.FullName))

        .ForMember(d => d.AppealTypeArrive, c => c.MapFrom(e => e.AppealTypeArrive.Translates.AsQueryable()
                .FirstOrDefault(AppealTypeArriveTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealTypeArrive.FullName))

            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
               .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Status.FullName))

            .ForMember(d => d.Region, c => c.MapFrom(e => e.Region.Translates.AsQueryable()
               .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Region.FullName))

            .ForMember(d => d.District, c => c.MapFrom(e => e.District.Translates.AsQueryable()
               .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.District.FullName))


        .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CallCenterAppealReject) != null ? StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.REJECTED) : false))
               .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CallCenterAppealSign) && StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
        .ForMember(x => x.CanSendEdoc, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CallCenterAppealSendToEdoc) != null ? StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.IN_EXECUTION) : false))
               .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CallCenterAppealSign) && StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
             .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CallCenterAppealEdit) != null
                ? StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.MODIFIED)
                    && (ent.StatusId == StatusIdConst.IN_EXECUTION
                        ? ent.EdocIncomingDocInfo.ProcessId == 27//Qoralamadagi status
                        : true)
                : false))
             .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.CallCenterAppealDelete) != null ? StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.DELETED) : false));



}
