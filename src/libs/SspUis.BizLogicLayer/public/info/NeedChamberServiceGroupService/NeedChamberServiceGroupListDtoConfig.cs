using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceGroupListDtoConfig : PerDtoConfig<NeedChamberServiceGroupListDto, NeedChamberServiceGroup>
    {
        public override Action<IMappingExpression<NeedChamberServiceGroup, NeedChamberServiceGroupListDto>> AlterReadMapping =>
            cfg => cfg
            .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NeedChamberServiceGroupTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.ShortName))

            .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(NeedChamberServiceGroupTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.FullName))

            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.State.FullName))
            ;
    }
}