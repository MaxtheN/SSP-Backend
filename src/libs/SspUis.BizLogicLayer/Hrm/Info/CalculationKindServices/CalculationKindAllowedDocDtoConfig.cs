using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{

    public class CalculationKindAllowedDocDtoConfig : PerDtoConfig<CalculationKindAllowedDocDto, CalculationKindAllowedDoc>
    {
        public override Action<IMappingExpression<CalculationKindAllowedDoc, CalculationKindAllowedDocDto>> AlterReadMapping => 
            cfg => cfg
            .ForMember(x => x.State,x => x.MapFrom(ent => ent.State.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name,ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName));
    }
}
