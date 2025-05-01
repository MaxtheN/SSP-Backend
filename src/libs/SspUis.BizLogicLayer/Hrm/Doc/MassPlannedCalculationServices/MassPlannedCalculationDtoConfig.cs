using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class MassPlannedCalculationDtoConfig : PerDtoConfig<MassPlannedCalculationDto, MassPlannedCalculation>
{
    public override Action<IMappingExpression<MassPlannedCalculation, MassPlannedCalculationDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
         .ForMember(x => x.OrgSettlementAccountCode, x => x.MapFrom(ent => ent.OrgSettlementAccount.AccountCode))
            .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
            .ForMember(x => x.OrgSettlementAccount, x => x.MapFrom(ent => ent.OrgSettlementAccount.AccountName))
        .ForMember(d => d.CalculationKind, c => c.MapFrom(e => e.CalculationKind.Translates.AsQueryable()
                .FirstOrDefault(CalculationKindTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.CalculationKind.FullName))
        .ForMember(d => d.RoundingType, c => c.MapFrom(e => e.RoundingType.Translates.AsQueryable()
                .FirstOrDefault(RoundingTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.RoundingType.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName));


}
