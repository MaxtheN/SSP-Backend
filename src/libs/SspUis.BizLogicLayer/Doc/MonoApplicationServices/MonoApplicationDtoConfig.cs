using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public class MonoApplicationDtoConfig : PerDtoConfig<MonoApplicationDto, MonoApplication>
    {
        public override Action<IMappingExpression<MonoApplication, MonoApplicationDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.MonoDistrict, x => x.MapFrom(ent => ent.MonoDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.MonoDistrict.FullName))

                .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                    .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Currency.FullName))

                 .ForMember(x => x.MonoRegion, x => x.MapFrom(ent => ent.MonoRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.MonoRegion.FullName))

                 .ForMember(x => x.MonoMfy, x => x.MapFrom(ent => ent.MonoMfy.FullName))
                 .ForMember(x => x.Mfy, x => x.MapFrom(ent => ent.Mfy.FullName))

                .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
                .ForMember(d => d.StudentTables, c => c.MapFrom(e => e.StudentTables))
                .ForMember(d => d.ItemTables, c => c.MapFrom(e => e.ItemTables));
    }
}
