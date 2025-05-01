using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public class ContractorListDtoConfig : PerDtoConfig<ContractorListDto, Contractor>
    {
        public override Action<IMappingExpression<Contractor, ContractorListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.InnOrPinfl, x => x.MapFrom(ent => ent.Pinfl ?? ent.Inn))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Oked.Translates.AsQueryable()
                    .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Oked.FullName))
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Oked.Parent.Parent.Parent.Code))
                .ForMember(x => x.Bank, x => x.MapFrom(ent => ent.Bank.Translates.AsQueryable()
                    .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Bank.BankName))
                .ForMember(x => x.Country, x => x.MapFrom(ent => ent.Country.Translates.AsQueryable()
                    .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Country.FullName))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
                .ForMember(x => x.Email, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.EMAIL).Contact))
                //.ForMember(x => x.WorkPhoneNumber, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.WORK_PHONE).Contact))
            .ForMember(x => x.WorkPhoneNumber, x => x.MapFrom(ent => ent.BusinessmanUserInContractors.FirstOrDefault().BusinessmanUser.UserName))
                .ForMember(x => x.MobilePhoneNumber, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.MOBILE_PHONE).Contact))
                .ForMember(x => x.AdditionalPhoneNumber, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.ADDITIONAL_PHONE).Contact))
                .ForMember(x => x.Faks, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.FAKS).Contact))
                .ForMember(x => x.Skype, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.SKYPE).Contact))
                .ForMember(x => x.Facebook, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.FACEBOOK).Contact))
                .ForMember(x => x.Telegram, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.TELEGRAM).Contact))
                .ForMember(x => x.WebSite, x => x.MapFrom(ent => ent.Contacts.OrderByDescending(a=>a.Id).FirstOrDefault(a=>a.ContactTypeId == ContactTypeIdConst.WEB_SITE).Contact))
                ;
    }
    public class ApplicationsOfContractorDtoConfig : PerDtoConfig<ApplicationsOfContractorDto, Application>
    {
        public override Action<IMappingExpression<Application, ApplicationsOfContractorDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                        .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? ent.Status.FullName))

                .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
                        .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? ent.ApplicationType.FullName))
                ;
    }
}
