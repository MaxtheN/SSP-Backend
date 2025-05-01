using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestDtoConfig : PerDtoConfig<SubsidyRequestDto, SubsidyRequest>
{
    public override Action<IMappingExpression<SubsidyRequest, SubsidyRequestDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.TotalSubsidyAmount, x => x.MapFrom(x => x.Tables.Sum(t => t.Subsidy)))
        .ForMember(x => x.ContractorSettlementAccount, x => x.MapFrom(x => x.ContractorSettlementAccount.AccountCode))
        .ForMember(x => x.Contractor, x => x.MapFrom(x => x.Contractor.FullName))
        .ForMember(x => x.ContractorInn, x => x.MapFrom(x => x.Contractor.Inn))
        .ForMember(x => x.ContractorPinfl, x => x.MapFrom(x => x.Contractor.Pinfl))
        .ForMember(x => x.Bank, x => x.MapFrom(x => x.ContractorSettlementAccount.Bank.Code + " - " + x.ContractorSettlementAccount.Bank.BankName))
        .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
        .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
        .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
        .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
		.ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
	;
}
