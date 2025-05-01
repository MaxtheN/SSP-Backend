using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipContractDtoConfig : PerDtoConfig<MemshipContractDto, MemshipContract>
{
    public override Action<IMappingExpression<MemshipContract, MemshipContractDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(
                StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name
                    , ServiceProvider.CultureHelper.CurrentCulture.Id)
            ).TranslateText ?? e.Status.FullName))

        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
            .FirstOrDefault(
            OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name
                , ServiceProvider.CultureHelper.CurrentCulture.Id)
            ).TranslateText ?? ent.Organization.FullName))

        .ForMember(x => x.MemshipContractType, x => x.MapFrom(ent => ent.MemshipContractType.Translates.AsQueryable()
            .FirstOrDefault(
            MemshipContractTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name
                , ServiceProvider.CultureHelper.CurrentCulture.Id)
            ).TranslateText ?? ent.MemshipContractType.FullName))

        .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
        .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
        .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
        .ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
        .ForMember(x => x.Address, x => x.MapFrom(ent => ent.Contractor.Address))
        .ForMember(x => x.Inn, x => x.MapFrom(ent => ent.Contractor.Inn))
        .ForMember(x => x.Pinfl, x => x.MapFrom(ent => ent.Contractor.Pinfl))
        .ForMember(x => x.MemshipApplicationId, x => x.MapFrom(ent => ent.Application.MemshipApplication.Id))
        .ForMember(x => x.MemshipApplicationId2, x => x.MapFrom(ent => ent.Application.Id2))
        .ForMember(x => x.BankId, x => x.MapFrom(ent => ent.Contractor.BankId))
        .ForMember(x=>x.RejectMessage , x=>x.MapFrom(ent => ent.RejectMessage))
        .ForMember(x=>x.RejectDate , x=>x.MapFrom(ent => ent.RejectDate))
        .ForMember(x => x.Bank, x => x.MapFrom(ent => ent.Contractor.Bank.Translates.AsQueryable()
            .FirstOrDefault(
            BankTranslate.GetExpr(DataLayer.BankTranslateColumn.bank_name
                , ServiceProvider.CultureHelper.CurrentCulture.Id)
            ).TranslateText ?? ent.Contractor.Bank.BankName))

        .ForMember(d => d.OrganizationSettlementAccount, c => c.MapFrom(e => e.OrganizationSettlementAccount.AccountName))
        .ForMember(d => d.ContractorSettlementAccount, c => c.MapFrom(e => e.ContractorSettlementAccount.AccountName))
        .ForMember(d => d.Files, c => c.MapFrom(e => e.Files));

}
