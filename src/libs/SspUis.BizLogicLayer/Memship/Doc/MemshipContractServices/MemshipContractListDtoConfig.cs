using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipContractListDtoConfig : PerDtoConfig<MemshipContractListDto, MemshipContract>
{
    public override Action<IMappingExpression<MemshipContract, MemshipContractListDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
            .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.MemshipContractType, x => x.MapFrom(ent => ent.MemshipContractType.Translates.AsQueryable()
            .FirstOrDefault(MemshipContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MemshipContractType.FullName))
        .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
        .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
        .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
        .ForMember(x => x.ContractorPinfl, x => x.MapFrom(ent => ent.Contractor.Pinfl))
        .ForMember(x => x.RegionalOrganizationId, x => x.MapFrom(ent => ent.RegionalOrganizationId))
        .ForMember(x => x.MemshipApplicationId, x => x.MapFrom(ent => ent.Application.MemshipApplication.Id))
        .ForMember(x => x.ContractorOked, x => x.MapFrom(ent => ent.Contractor.Oked.Code + " - " + ent.Contractor.Oked.FullName))
        .ForMember(x => x.ContractorCategoryId, x => x.MapFrom(ent => ent.Application.MemshipApplication.ContractorCategoryId))
        .ForMember(x => x.ContractorCategory, x => x.MapFrom(ent => ent.Application.MemshipApplication.ContractorCategory.Translates.AsQueryable()
            .FirstOrDefault(ContractorCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.MemshipApplication.ContractorCategory.FullName))
        .ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
        .ForMember(d => d.OrganizationSettlementAccount, c => c.MapFrom(e => e.OrganizationSettlementAccount.AccountName))
        .ForMember(d => d.ContractorSettlementAccount, c => c.MapFrom(e => e.ContractorSettlementAccount.AccountName))

        .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
        .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
            .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))

        .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.CANCELED)
                        : StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.CANCELED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipCertificateCancel)))

        //.ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
        //                ? StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.REJECTED)
        //                : StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.REJECTED)
        //                          && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipContractReject)))

        .ForMember(x => x.HasCertificate, x => x.MapFrom(ent => ent.Certificates
            .Count(certificate => certificate.StatusId != StatusIdConst.DELETED) > 0))

        .ForMember(x => x.HasCertificateCanceled, x => x.MapFrom(ent => ent.Certificates
            .Count(certificate => certificate.StatusId == StatusIdConst.CANCELED) > 0))

        .ForMember(x => x.Opf, x => x.MapFrom(ent => ent.Contractor.Opf.Translates.AsQueryable()
            .FirstOrDefault(OpfTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Opf.FullName))

        .ForMember(x => x.OpfId, x => x.MapFrom(ent => ent.Contractor.OpfId))

        .ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.SIGNING)
                        : StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.SIGNED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipContractSign)))

        .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.MODIFIED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipContractEdit)))

        .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, StatusIdConst.DELETED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipContractDelete)))

        .ForMember(x => x.CanCreateCertificate, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : ent.StatusId == StatusIdConst.SIGNED
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipCertificateCreate)
                            && (!ent.Certificates.Any() || (ent.Certificates.Any(x => x.StatusId == StatusIdConst.CANCELED) && ent.Certificates.Any(x => x.StatusId != StatusIdConst.FORMED)))))

        .ForMember(x => x.CanCreateAdditionalAgreement, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : ent.StatusId == StatusIdConst.SIGNING || ent.StatusId == StatusIdConst.SIGNED))

        .ForMember(x => x.CanChangeDocnumber,
           x => x.MapFrom(ent => !ent.Certificates.Any(certificate => certificate.StatusId == StatusIdConst.FORMED)))
;
}
