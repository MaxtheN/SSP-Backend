using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.FixedMinimumValueServices
{
    public class FixedMinimumValueListDtoConfig : PerDtoConfig<FixedMinimumValueListDto, FixedMinimumValue>
    {
        public override Action<IMappingExpression<FixedMinimumValue, FixedMinimumValueListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.MinimumValueType, x => x.MapFrom(ent => ent.MinimumValueType.Translates.AsQueryable()
                .FirstOrDefault(MinimumValueTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MinimumValueType.FullName))
            ;
    }
}
