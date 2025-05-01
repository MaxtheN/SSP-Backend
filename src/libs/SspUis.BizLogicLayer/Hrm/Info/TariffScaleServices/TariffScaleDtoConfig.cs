using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices;

public class TariffScaleDtoConfig : PerDtoConfig<TariffScaleDto, TariffScale>
{
    public override Action<IMappingExpression<TariffScale, TariffScaleDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        .ForMember(x => x.MinimumValueType, x => x.MapFrom(ent => ent.MinimumValueType.Translates.AsQueryable()
            .FirstOrDefault(MinimumValueTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MinimumValueType.FullName))
        ;
}
