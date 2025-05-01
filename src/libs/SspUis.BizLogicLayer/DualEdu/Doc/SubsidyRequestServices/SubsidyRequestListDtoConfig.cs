using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestListDtoConfig : PerDtoConfig<SubsidyRequestListDto, SubsidyRequest>
{
    public override Action<IMappingExpression<SubsidyRequest, SubsidyRequestListDto>> AlterReadMapping => cfg => cfg
     .ForMember(x => x.TotalSubsidyAmount, x => x.MapFrom(x => x.Tables.Sum(t => t.Subsidy)))
     .ForMember(x => x.Contractor, x => x.MapFrom(x => x.Contractor.FullName))
     .ForMember(x => x.ContractorInnPinfl, x => x.MapFrom(x => (x.Contractor.Inn != null) ? x.Contractor.Inn : x.Contractor.Pinfl))
     //.ForMember(x => x.TotalEntitlementAmount, x => x.MapFrom(x => x.Tables.Sum(t => t.EntitlementAmount)))
     .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
     .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
     .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
     .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
     //.ForMember(x => x.CanEdit, x => x.MapFrom(x => ServiceProvider.AuthService.Contractor != null
     //       ? StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.MODIFIED)
     //       : false))
     //.ForMember(x => x.CanDelete, x => x.MapFrom(x => StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.MODIFIED)))
     //.ForMember(x => x.CanSend, x => x.MapFrom(x => ServiceProvider.AuthService.Contractor != null
     //       ? StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.SENT)
     //       : false))
     //.ForMember(x => x.CanRevoke, x => x.MapFrom(x => ServiceProvider.AuthService.Contractor != null
     //       ? StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.REVOKED)
     //       : false))
     //.ForMember(x => x.CanAccept, x => x.MapFrom(x => ServiceProvider.AuthService.Contractor != null
     //       ? false
     //       : StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.ACCEPTED)))
     //.ForMember(x => x.CanCancel, x => x.MapFrom(x => ServiceProvider.AuthService.Contractor != null
     //       ? false
     //       : StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.CANCELED)))
     //.ForMember(x => x.CanReject, x => x.MapFrom(x => ServiceProvider.AuthService.Contractor != null
     //       ? false
     //       : StatusIdConst.CanSubsidyRequestApplyStatus(x.StatusId, StatusIdConst.REJECTED)))
        ;
}
