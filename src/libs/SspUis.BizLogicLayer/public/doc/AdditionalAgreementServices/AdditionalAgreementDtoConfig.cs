using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;

public class AdditionalAgreementDtoConfig : PerDtoConfig<AdditionalAgreementDto, AdditionalAgreement>
{
    public override Action<IMappingExpression<AdditionalAgreement, AdditionalAgreementDto>> AlterReadMapping =>
        cfg => cfg
            //.ForMember(x => x.MemshipContractDocNumber, cfg => cfg.MapFrom(ent => ent.MemshipContract.DocNumber))
            //.ForMember(x => x.MemshipContractDocOn, cfg => cfg.MapFrom(ent => ent.MemshipContract.DocOn))
            .ForMember(x => x.Status, x => x.MapFrom(ent =>
                    ent.Status.Translates.AsQueryable().FirstOrDefault(
                        StatusTranslate.GetExpr(
                            TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Status.FullName))
             .ForMember(x => x.Region, x => x.MapFrom(ent =>
                    ent.Contractor.Region.Translates.AsQueryable().FirstOrDefault(
                        RegionTranslate.GetExpr(
                            TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Contractor.Region.FullName))

            .ForMember(x => x.Contractor, x => x.MapFrom(ent =>
                    ent.Contractor.FullName))

            .ForMember(x => x.ApplicationType, x => x.MapFrom(ent =>
                    ent.ApplicationType.Translates.AsQueryable().FirstOrDefault(
                        ApplicationTypeTranslate.GetExpr(
                            TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.ApplicationType.FullName))

            .ForMember(x => x.Organization, x => x.MapFrom(ent =>
                    ent.Organization.Translates.AsQueryable().FirstOrDefault(
                        OrganizationTranslate.GetExpr(
                            TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Organization.FullName))

            .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, StatusIdConst.SIGNED)
                        : StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, StatusIdConst.SIGNING)))

            .ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, StatusIdConst.REJECTED)))
;
}
