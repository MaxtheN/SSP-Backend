using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class PlannedCalculationTableDtoConfig : PerDtoConfig<PlannedCalculationTableDto, PlannedCalculationTable>
{
    public override Action<IMappingExpression<PlannedCalculationTable, PlannedCalculationTableDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Position, c => c.MapFrom(e => e.Position.FullName))
        .ForMember(d => d.TempCalcKindType, c => c.MapFrom(e => e.TempCalcKindType.Translates.AsQueryable()
                .FirstOrDefault(TempCalcKindTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.TempCalcKindType.FullName))
            .ForMember(d => d.Employee, c => c.MapFrom(e => e.Employee.Person.FullName));
}
