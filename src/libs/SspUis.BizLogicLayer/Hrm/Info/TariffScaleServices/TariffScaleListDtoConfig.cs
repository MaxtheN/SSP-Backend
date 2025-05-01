using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices;

public class TariffScaleListDtoConfig : PerDtoConfig<TariffScaleListDto, TariffScale>
{
    public override Action<IMappingExpression<TariffScale, TariffScaleListDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
        .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        .ForMember(x => x.MinimumValueType, x => x.MapFrom(ent => ent.MinimumValueType.Translates.AsQueryable()
            .FirstOrDefault(MinimumValueTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MinimumValueType.FullName))
        .ForMember(x => x.TariffScaleType, x => x.MapFrom(ent => ent.TariffScaleType.Translates.AsQueryable()
            .FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScaleType.FullName));
}
