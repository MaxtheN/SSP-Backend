using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class MemshipAdditionalAgreementDtoConfig : PerDtoConfig<MemshipAdditionalAgreementDto, AdditionalAgreement>
{
    public override Action<IMappingExpression<AdditionalAgreement, MemshipAdditionalAgreementDto>> AlterReadMapping =>
    cfg => cfg
        .ForMember(x => x.OrganizationAddress, cfg => cfg.MapFrom(ent => ent.Organization.Address))
        .ForMember(x => x.OrganizationAccountCode, cfg => cfg.MapFrom(ent => ent.MemshipContract.OrganizationSettlementAccount.AccountCode))
        .ForMember(x => x.OrganizationBank, cfg => cfg.MapFrom(ent => ent.MemshipContract.OrganizationSettlementAccount.Bank.BankName))
        .ForMember(x => x.OrganizationMFO, cfg => cfg.MapFrom(ent => ent.MemshipContract.OrganizationSettlementAccount.Bank.Code))
        .ForMember(x => x.OrganizationInn, cfg => cfg.MapFrom(ent => ent.MemshipContract.Organization.Inn))
        .ForMember(x => x.OrganizationOked, cfg => cfg.MapFrom(ent => ent.MemshipContract.Organization.Oked.Code))
        .ForMember(x => x.OrganizationPhone, cfg => cfg.MapFrom(ent => ent.MemshipContract.Organization.PhoneNumber))
        .ForMember(x => x.OrganizationDirector, cfg => cfg.MapFrom(ent => ent.MemshipContract.Organization.Director))
        .ForMember(x => x.ContractorAddress, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.Address))
        .ForMember(x => x.ContractorAccountCode, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.SettlementAccounts.FirstOrDefault(x => x.IsMain).AccountCode))
        .ForMember(x => x.ContractorBank, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.SettlementAccounts.FirstOrDefault(x => x.IsMain).Bank.BankName))
        .ForMember(x => x.ContractorMFO, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.SettlementAccounts.FirstOrDefault(x => x.IsMain).Bank.Code))
        .ForMember(x => x.ContractorInn, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.Inn))
        .ForMember(x => x.ContractorOked, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.Oked.Code))
        .ForMember(x => x.ContractorPhone, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.PhoneNumber))
        .ForMember(x => x.ContractorDirector, cfg => cfg.MapFrom(ent => ent.MemshipContract.Contractor.Director ?? ""))

            .ForMember(x => x.MemshipContractDocNumber, cfg => cfg.MapFrom(ent => ent.MemshipContract.DocNumber))
            .ForMember(x => x.MemshipContractDocOn, cfg => cfg.MapFrom(ent => ent.MemshipContract.DocOn))
            .ForMember(x => x.MemshipContractSignedAt, cfg => cfg.MapFrom(ent => ent.MemshipContract.Signs.FirstOrDefault(s => s.StatusId == StatusIdConst.SIGNED).SignedAt))
            .ForMember(x => x.Status, x => x.MapFrom(ent =>
                    ent.Status.Translates.AsQueryable().FirstOrDefault(
                        StatusTranslate.GetExpr(
                            TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Status.FullName))

            .ForMember(x => x.Contractor, x => x.MapFrom(ent =>
                    ent.Contractor.FullName))

            .ForMember(x => x.Region, x => x.MapFrom(ent =>
                    ent.Contractor.Region.Translates.AsQueryable().FirstOrDefault(
                        RegionTranslate.GetExpr(
                            TranslateColumn.full_name,
                            ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Contractor.Region.FullName))

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
