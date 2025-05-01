using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices;

public class TariffScaleCoefDtoConfig : PerDtoConfig<TariffScaleCoefDto, TariffScaleCoef>
{
    public override Action<IMappingExpression<TariffScaleCoef, TariffScaleCoefDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        .ForMember(x => x.TariffScale, x => x.MapFrom(ent => ent.TariffScale.Translates.AsQueryable()
            .FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScale.FullName))
        ;
}
