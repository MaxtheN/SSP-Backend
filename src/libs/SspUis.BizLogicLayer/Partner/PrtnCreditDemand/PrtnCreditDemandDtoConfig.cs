using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices
{
    public class PrtnCreditDemandDtoConfig : PerDtoConfig<PrtnCreditDemandDto, PrtnCreditDemand>
    {
        public override Action<IMappingExpression<PrtnCreditDemand, PrtnCreditDemandDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.PrtnCertificateDocNumber, x => x.MapFrom(ent => ent.PrtnCertificate.DocNumber))
                .ForMember(x => x.PrtnCertificateDocOn, x => x.MapFrom(ent => ent.PrtnCertificate.DocOn))
                .ForMember(x => x.NewVacanciesCount, x => x.MapFrom(ent => ent.PrtnCertificate.PrtnContract.NewVacanciesCount))
                .ForMember(x => x.Address, x => x.MapFrom(ent => ent.Contractor.Address))
                .ForMember(x => x.Bank, x => x.MapFrom(ent => ent.Bank.Translates.AsQueryable()
                    .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Bank.BankName))
             .ForMember(x => x.RegionId, x => x.MapFrom(ent => ent.PrtnApplication.ChooseLocation ? ent.PrtnApplication.ChoosedRegionId : ent.Contractor.RegionId))
            .ForMember(x => x.DistrictId, x => x.MapFrom(ent => ent.PrtnApplication.ChooseLocation ? ent.PrtnApplication.ChoosedDistrictId : ent.Contractor.DistrictId))
                .ForMember(x => x.RegionName, x => x.MapFrom(ent => ent.PrtnApplication.ChooseLocation ?
                    ent.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.ChoosedRegion.FullName
                    : ent.Contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Region.FullName))
            .ForMember(x => x.DistrictName, x => x.MapFrom(ent => ent.PrtnApplication.ChooseLocation ?
                    ent.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.ChoosedDistrict.FullName
                    : ent.Contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.District.FullName))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnCertificate.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnCertificate.PrtnContractType.FullName))
                .ForMember(x => x.PrtnContractTypeId, x => x.MapFrom(ent => ent.PrtnCertificate.PrtnContractTypeId))
                .ForMember(x => x.BusinessmanUser, x => x.MapFrom(ent => ent.BusinessmanUser.FullName))
            ;

    }
}
