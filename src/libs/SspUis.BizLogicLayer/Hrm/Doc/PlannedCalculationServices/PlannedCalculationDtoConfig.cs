using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class PlannedCalculationDtoConfig : PerDtoConfig<PlannedCalculationDto, PlannedCalculation>
{
    public override Action<IMappingExpression<PlannedCalculation, PlannedCalculationDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(d => d.CalculationKind, c => c.MapFrom(e => e.CalculationKind.Translates.AsQueryable()
                .FirstOrDefault(CalculationKindTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.CalculationKind.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName));


}
