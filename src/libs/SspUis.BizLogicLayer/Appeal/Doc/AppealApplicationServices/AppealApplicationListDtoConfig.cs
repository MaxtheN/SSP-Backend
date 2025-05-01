using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;

namespace SspUis.BizLogicLayer.Appeal;

public class AppealApplicationListDtoConfig : PerDtoConfig<AppealApplicationListDto, AppealApplication>
{
    public override Action<IMappingExpression<AppealApplication, AppealApplicationListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Contractor, c => c.MapFrom(e => e.Contractor.FullName))
            .ForMember(d => d.Department, c => c.MapFrom(e => e.Department.FullName))
            .ForMember(d => d.AppealType, c => c.MapFrom(e => e.AppealType.FullName))
            .ForMember(d => d.PersonName, c => c.MapFrom(e => e.Person.FullName ?? e.PersonFullName))
            .ForMember(d => d.ContractorInn, c => c.MapFrom(e => e.Contractor.Pinfl ?? e.Contractor.Inn))
            .ForMember(d => d.PhoneNumber, c => c.MapFrom(e => e.PhoneNumber))
            .ForMember(d => d.AppealFormatType, c => c.MapFrom(e => e.AppealFormatType.FullName))
            .ForMember(d => d.RegionId, c => c.MapFrom(e => e.RegionId))

            .ForMember(d => d.AppealTypeArrive, c => c.MapFrom(e => e.AppealTypeArrive.Translates.AsQueryable()
                .FirstOrDefault(AppealTypeArriveTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealTypeArrive.FullName))

            .ForMember(d => d.AppealDescription, c => c.MapFrom(e => e.AppealDescription.Translates.AsQueryable()
                .FirstOrDefault(AppealDescriptionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.AppealDescription.FullName))

            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
               .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Status.FullName))

            .ForMember(d => d.Region, c => c.MapFrom(e => e.Region.Translates.AsQueryable()
               .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Region.FullName))

            .ForMember(d => d.District, c => c.MapFrom(e => e.District.Translates.AsQueryable()
               .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.District.FullName))


            .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppealApplicationReject) != null ? StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.REJECTED) : false))
            .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppealApplicationSign) && StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED) && ent.StatusId != StatusIdConst.SIGNING ? true : false))
            .ForMember(x => x.CanSendEdoc, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppealApplicationSendToEdoc) != null ? StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.IN_EXECUTION) : false))
            .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppealApplicationSign) && StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED) && ent.StatusId != StatusIdConst.SIGNING && ent.EdocInfoForList.ProcessId == 27 ? true : false))
            .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppealApplicationEdit) != null ? StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.MODIFIED) : false))
            .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.AppealApplicationDelete) != null ? StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.DELETED) : false))
            .ForMember(x => x.EdocInfoForList, x => x.MapFrom(ent => ent.EdocInfoForList))
            ;
}
