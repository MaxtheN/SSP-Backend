using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ClaimApplicationDtoConfig : PerDtoConfig<ClaimApplicationDto, ClaimApplication>
    {
        public override Action<IMappingExpression<ClaimApplication, ClaimApplicationDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ClaimApplicationType, x => x.MapFrom(ent => ent.ClaimApplicationType.Translates.AsQueryable()
                    .FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.ClaimApplicationType.FullName))
                .ForMember(x=>x.ContractorInn, x=>x.MapFrom(ent=>ent.MemshipContract.Contractor.Inn))
                .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                    .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Currency.FullName))

                 .ForMember(x => x.ClaimTheme, x => x.MapFrom(ent => ent.ClaimTheme.Translates.AsQueryable()
                    .FirstOrDefault(ClaimThemeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.ClaimTheme.FullName))

                .ForMember(x => x.MemshipContractDocNumber, x => x.MapFrom(ent => ent.MemshipContract.DocNumber))
                .ForMember(x => x.MemshipContractDocOn, x => x.MapFrom(ent => ent.MemshipContract.DocOn))

                .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
                .ForMember(d => d.Tables, c => c.MapFrom(e => e.Tables));
    }
}
