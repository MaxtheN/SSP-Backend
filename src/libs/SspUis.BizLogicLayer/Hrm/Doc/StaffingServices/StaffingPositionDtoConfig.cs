using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using SspUis.DataLayer;
using System.Linq;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class StaffingPositionDtoConfig : PerDtoConfig<StaffingPositionDto, StaffingPosition>
    {
        public override Action<IMappingExpression<StaffingPosition, StaffingPositionDto>> AlterReadMapping =>
             cfg => cfg
                .ForMember(x => x.OwnerDocDate, x => x.MapFrom(ent => ent.Owner.DocOn.AsDateTime()))
                .ForMember(x => x.DepartmentName, x => x.MapFrom(ent => ent.Department.FullName))
                .ForMember(x => x.DepartmentCode, x => x.MapFrom(ent => ent.Department.Code))
                .ForMember(x => x.PositionOrderCode, x => x.MapFrom(ent => ent.Position.OrderCode))
                .ForMember(x => x.PositionClassification, x => x.MapFrom(ent => ent.PositionClassification.Translates.AsQueryable().FirstOrDefault(PositionClassificationTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionClassification.ShortName))
                .ForMember(x => x.PositionName, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.FullName))
                .ForMember(x => x.PositionTypeName, x => x.MapFrom(ent => ent.PositionType.Translates.AsQueryable().FirstOrDefault(PositionTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionType.FullName))
                .ForMember(x => x.PositionCategoryName, x => x.MapFrom(ent => ent.PositionCategory.Translates.AsQueryable().FirstOrDefault(PositionCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionCategory.FullName))
                .ForMember(x => x.QualificationCategoryName, x => x.MapFrom(ent => ent.QualificationCategory.Translates.AsQueryable().FirstOrDefault(QualificationCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.QualificationCategory.FullName))
                .ForMember(x => x.TariffScaleName, x => x.MapFrom(ent => ent.TariffScale.Translates.AsQueryable().FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScale.FullName))
                .ForMember(x => x.RankName, x => x.MapFrom(ent => ent.RankId.HasValue ? $"{ent.RankCode} - {ent.RankCoef}" : ""))
                .ForMember(x => x.StaffingCalcKindNames, x => x.MapFrom(ent => string.Join(", ", ent.CalcKinds.Select(b => $"{b.CalculationKind.Translates.AsQueryable().FirstOrDefault(CalculationKindTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? b.CalculationKind.FullName} ({b.CalcSum})"))))
                .ForMember(x => x.CalcKinds, x => x.MapFrom(ent => ent.CalcKinds))
                .ForMember(x => x.TariffScaleTypeName, x => x.MapFrom(ent => ent.Position.TariffScaleType.Translates.AsQueryable().FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.TariffScaleType.FullName))
                .ForMember(x => x.CalcKindsCalcSum, x => x.MapFrom(ent => ent.CalcKinds.Sum(a => a.CalcSum)))
                .ForMember(x => x.PositionPeriod, x => x.MapFrom(ent => ent.PositionPeriod.Translates.AsQueryable().FirstOrDefault(PositionPeriodTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionPeriod.FullName))
                .ForMember(x => x.PositionPeriodMonthCount, x => x.MapFrom(ent => ent.PositionPeriod.MonthCount))
            ;

    }
}
