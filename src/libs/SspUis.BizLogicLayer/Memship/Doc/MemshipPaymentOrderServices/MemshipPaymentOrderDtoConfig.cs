using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipPaymentOrderDtoConfig : PerDtoConfig<MemshipPaymentOrderDto, MemshipPaymentOrder>
{
    public override Action<IMappingExpression<MemshipPaymentOrder, MemshipPaymentOrderDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.Contractor, x => x.MapFrom(x => x.Contractor.FullName))
        .ForMember(x => x.MemshipContractNumber, x => x.MapFrom(x => x.MemshipContract.DocNumber))
        .ForMember(x => x.MemshipContractDocOn, x => x.MapFrom(x => x.MemshipContract.DocOn))
        .ForMember(x => x.BankName, x => x.MapFrom(ent => ent.Bank.Translates.AsQueryable().FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Bank.BankName))
        .ForMember(x => x.BankCode, x => x.MapFrom(x => x.Bank.BankCode.Code))
        .ForMember(x => x.ServiceContractDocOn, x => x.MapFrom(x => x.ServiceContract.DocOn))
        .ForMember(x => x.ServiceContractNumber, x => x.MapFrom(x => x.ServiceContract.DocNumber))
        .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable().FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
        .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable().FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Currency.FullName))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
        //.ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
     ;
}
