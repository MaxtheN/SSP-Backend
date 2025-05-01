using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices;

public class PartisanshipListDtoConfig : PerDtoConfig<PartisanshipListDto, Partisanship>
{
    public override Action<IMappingExpression<Partisanship, PartisanshipListDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(PartisanshipTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
        .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(PartisanshipTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        ;
}
