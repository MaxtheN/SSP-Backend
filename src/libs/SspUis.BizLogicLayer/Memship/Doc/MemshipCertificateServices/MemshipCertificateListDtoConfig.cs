using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class MemshipCertificateListDtoConfig : PerDtoConfig<MemshipCertificateListDto, MemshipCertificate>
{
    public override Action<IMappingExpression<MemshipCertificate, MemshipCertificateListDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(d => d.Region, c => c.MapFrom(e => e.Region.Translates.AsQueryable()
        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Region.FullName))
        .ForMember(d => d.District, c => c.MapFrom(e => e.District.Translates.AsQueryable()
        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.District.FullName))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
            .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
        .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
        .ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
        .ForMember(x => x.PhoneNumber, x => x.MapFrom(ent => ent.MemshipContract
                .Application.MemshipApplication.ContractorWorkPhoneNumber ??
                ent.MemshipContract.Application.MemshipApplication.ContractorMobilePhoneNumber??
		        ent.MemshipContract.Application.MemshipApplication.ContractorAdditionalPhoneNumber))
        .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
        .ForMember(x => x.ContractorOked, x => x.MapFrom(ent => ent.Contractor.Oked.Code + " - " + ent.Contractor.Oked.FullName))
        .ForMember(x => x.ContractorOkedId, x => x.MapFrom(ent => ent.Contractor.Oked.Id))
        .ForMember(x => x.ContractorPinfl, x => x.MapFrom(ent => ent.Contractor.Pinfl))
        .ForMember(x => x.ContractorCategoryId, x => x.MapFrom(ent => ent.MemshipContract.Application.MemshipApplication.ContractorCategoryId))
        .ForMember(x => x.ContractorCategory, x => x.MapFrom(ent => ent.MemshipContract.Application.MemshipApplication.ContractorCategory.FullName))
        .ForMember(x => x.MemshipContractTypeId, x => x.MapFrom(ent => ent.MemshipContract.MemshipContractTypeId))
        .ForMember(x => x.MemshipContractType, x => x.MapFrom(ent => ent.MemshipContract.MemshipContractType.Translates.AsQueryable()
            .FirstOrDefault(MemshipContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MemshipContract.MemshipContractType.FullName))
        .ForMember(d => d.ContractorSettlementAccount, c => c.MapFrom(e => e.ContractorSettlementAccount.AccountName))
        .ForMember(d => d.MemshipApplicationId, c => c.MapFrom(e => e.MemshipContract.Application.MemshipApplication.Id))

        .ForMember(x => x.Opf, x => x.MapFrom(ent => ent.Contractor.Opf.Translates.AsQueryable()
            .FirstOrDefault(OpfTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Opf.FullName))
        .ForMember(x => x.OpfId, x => x.MapFrom(ent => ent.Contractor.OpfId))

        .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                ? false
                : StatusIdConst.CanMemshipApplicationApplyStatus(ent.StatusId, StatusIdConst.ACCEPTED)
                          && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipCertificateAccept)))

        .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                ? false
                : StatusIdConst.CanMemshipApplicationApplyStatus(ent.StatusId, StatusIdConst.CANCELED)
                          && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipCertificateCancel)))

        .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                ? false
                : StatusIdConst.CanMemshipApplicationApplyStatus(ent.StatusId, StatusIdConst.MODIFIED)
                    && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipCertificateEdit)))

        .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                ? false
                : StatusIdConst.CanMemshipApplicationApplyStatus(ent.StatusId, StatusIdConst.DELETED)
                    && ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipCertificateDelete)))
        ;
}
