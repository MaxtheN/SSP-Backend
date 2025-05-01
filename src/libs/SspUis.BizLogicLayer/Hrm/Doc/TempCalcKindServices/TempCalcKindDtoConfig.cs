using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TempCalcKindDtoConfig : PerDtoConfig<TempCalcKindDto, TempCalcKind>
{
    public override Action<IMappingExpression<TempCalcKind, TempCalcKindDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(d => d.CalculationKind, c => c.MapFrom(e => e.CalculationKind.Translates.AsQueryable()
                .FirstOrDefault(CalculationKindTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.CalculationKind.FullName))
        .ForMember(d => d.TempCalcKindType, c => c.MapFrom(e => e.TempCalcKindType.Translates.AsQueryable()
                .FirstOrDefault(TempCalcKindTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.TempCalcKindType.FullName))
        .ForMember(d => d.Details, c => c.MapFrom(e => e.TempCalcKindType.FullName))
		.ForMember(d => d.Employees, c => c.MapFrom(ent => ent.Tables.Select(x => x.Employee.Person)))
		.ForMember(d => d.DocDetails, c => c.MapFrom(e => e.Details))
        .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))

		.ForMember(d => d.Region, c => c.MapFrom(e => e.Organization.Region.FullName));
}
