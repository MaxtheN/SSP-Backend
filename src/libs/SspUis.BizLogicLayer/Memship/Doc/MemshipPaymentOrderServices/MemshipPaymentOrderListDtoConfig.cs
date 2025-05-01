using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipPaymentOrderListDtoConfig : PerDtoConfig<MemshipPaymentOrderListDto, MemshipPaymentOrder>
{
    public override Action<IMappingExpression<MemshipPaymentOrder, MemshipPaymentOrderListDto>> AlterReadMapping => cfg => cfg
     .ForMember(x => x.Contractor, x => x.MapFrom(x => x.Contractor.FullName))
     .ForMember(x => x.ContractorInn, x => x.MapFrom(x => x.Contractor.Inn))
     .ForMember(x => x.MemshipContractNumber, x => x.MapFrom(x => x.MemshipContract.DocNumber))
     .ForMember(x => x.ServiceContractNumber, x => x.MapFrom(x => x.ServiceContract.DocNumber))
     .ForMember(x => x.MemshipContractDocOn, x => x.MapFrom(x => x.MemshipContract.DocOn))
     .ForMember(x => x.RegionalOrganization, x => x.MapFrom(x => x.MemshipContract.RegionalOrganization.FullName))
     .ForMember(x => x.ServiceContractDocOn, x => x.MapFrom(x => x.ServiceContract.DocOn))
     .ForMember(x => x.BankName, x => x.MapFrom(ent => ent.Bank.Translates.AsQueryable().FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Bank.BankName))
     .ForMember(x => x.BankCode, x => x.MapFrom(x => x.Bank.BankCode.Code))
     .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable().FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
     .ForMember(x => x.BankCode, x => x.MapFrom(x => x.Bank.BankCode.Code))
     .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable().FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Currency.FullName))
     .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
     .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))

     .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipPaymentOrderCancel)
            ? StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.CANCELED)
            : false))
     .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipPaymentOrderAccept)
            ? StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.ACCEPTED)
            : false))
     .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipPaymentOrderEdit) && StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.MODIFIED) ? true : false))
     .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.MemshipPaymentOrderDelete) && StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.DELETED) ? true : false))

     .ForMember(x => x.SrvCanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ServicePaymentOrderCancel) && StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.NOT_ACCEPTED) ? true : false))
     .ForMember(x => x.SrvCanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ServicePaymentOrderAccept) && StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.ACCEPTED) ? true : false))
     .ForMember(x => x.SrvCanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ServicePaymentOrderEdit) != null ? StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.MODIFIED) : false))
     .ForMember(x => x.SrvCanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.HasPermission(ModuleCode.ServicePaymentOrderDelete) != null ? StatusIdConst.CanApplySrvDocStatus(ent.StatusId, StatusIdConst.DELETED) : false))
        ;
}
