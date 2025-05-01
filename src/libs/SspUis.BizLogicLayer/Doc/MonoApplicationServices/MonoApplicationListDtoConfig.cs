using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    public class MonoApplicationListDtoConfig : PerDtoConfig<MonoApplicationListDto, MonoApplication>
    {
        public override Action<IMappingExpression<MonoApplication, MonoApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                  .ForMember(x => x.MonoDistrict, x => x.MapFrom(ent => ent.MonoDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.MonoDistrict.FullName))

            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Status.FullName))

                .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                        .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                        .TranslateText ?? ent.Currency.FullName))
                .ForMember(x => x.MonoAdress, x => x.MapFrom(ent => ent.MonoAdress))
                .ForMember(x => x.MonoMfy, x => x.MapFrom(ent => ent.MonoMfy.FullName))
                .ForMember(x => x.Mfy, x => x.MapFrom(ent => ent.Mfy.FullName))

                 .ForMember(x => x.MonoRegion, x => x.MapFrom(ent => ent.MonoRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.MonoRegion.FullName));
    }
}
