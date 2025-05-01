using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.SignCriterionService;

public class SignCriterionDtoConfig : PerDtoConfig<SignCriterionDto, SignCriterion>
{
    public override Action<IMappingExpression<SignCriterion, SignCriterionDto>> AlterReadMapping => cfg => cfg
    .ForMember(x => x.State, x => x.MapFrom(ent =>
            ent.State.Translates.AsQueryable().FirstOrDefault(
                StateTranslate.GetExpr(
                    TranslateColumn.full_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.State.FullName))

    .ForMember(x => x.ContractorCategory, x => x.MapFrom(ent =>
            ent.ContractorCategory.Translates.AsQueryable().FirstOrDefault(
                ContractorCategoryTranslate.GetExpr(
                    TranslateColumn.full_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.ContractorCategory.FullName))

    .ForMember(x => x.ApplicationType, x => x.MapFrom(ent =>
            ent.ApplicationType.Translates.AsQueryable().FirstOrDefault(
                ApplicationTypeTranslate.GetExpr(
                    TranslateColumn.full_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.ApplicationType.FullName))

    .ForMember(x => x.Position, x => x.MapFrom(ent =>
            ent.Position.Translates.AsQueryable().FirstOrDefault(
                PositionTranslate.GetExpr(
                    TranslateColumn.full_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.Position.FullName))

    .ForMember(x => x.OrganizationGroup, x => x.MapFrom(ent =>
            ent.OrganizationGroup.Translates.AsQueryable().FirstOrDefault(
                OrganizationGroupTranslate.GetExpr(
                    TranslateColumn.full_name,
                    ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? ent.OrganizationGroup.FullName));
}
