using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices;

public class SettlementAccountSourceDtoConfig : PerDtoConfig<SettlementAccountSourceDto, SettlementAccountSource>
{
    public override Action<IMappingExpression<SettlementAccountSource, SettlementAccountSourceDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        .ForMember(x => x.Parent, x => x.MapFrom(ent => ent.Parent == null ? "" : ent.Parent.Translates.AsQueryable()
            .FirstOrDefault(SettlementAccountSourceTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Parent.FullName))
        ;
}
