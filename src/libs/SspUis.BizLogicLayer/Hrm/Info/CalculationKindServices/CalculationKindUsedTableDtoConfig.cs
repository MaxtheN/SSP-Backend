using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindUsedTableDtoConfig : PerDtoConfig<CalculationKindUsedTableDto,CalculationKindUsedTable>
    {
        public override Action<IMappingExpression<CalculationKindUsedTable, CalculationKindUsedTableDto>> AlterReadMapping => cfg => cfg
                .ForMember(x => x.FormedCalculationKind,x => x.MapFrom(ent => ent.FormedCalculationKind.Translates.AsQueryable().FirstOrDefault(CalculationKindTranslate.GetExpr(TranslateColumn.full_name,ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FormedCalculationKind.FullName))
                .ForMember(x => x.MinimumValueType, x => x.MapFrom(ent => ent.MinimumValueTypeId != null ?ent.MinimumValueType.Translates.AsQueryable().FirstOrDefault(MinimumValueTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.MinimumValueType.FullName : null))
        ;
    }
}
