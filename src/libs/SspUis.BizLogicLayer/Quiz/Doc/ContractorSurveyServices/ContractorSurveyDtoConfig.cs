using AutoMapper;
using GenericServices.Configuration;
using SspUis;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Linq;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;


public class ContractorSurveyDtoConfig : PerDtoConfig<ContractorSurveyDto, ContractorSurvey>
{
    public override Action<IMappingExpression<ContractorSurvey, ContractorSurveyDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
         .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor == null ? null : ent.Contractor.FullName))
         .ForMember(x => x.Questionnaire, x => x.MapFrom(ent => ent.Questionnaire.Translates.AsQueryable()
                .FirstOrDefault(QuestionnaireTranslate.GetExpr(
                        TranslateQuestionnaire.title,
                        ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
         .ForMember(x => x.InspectionType, x => x.MapFrom(ent => ent.InspectionType.Translates.AsQueryable()
                .FirstOrDefault(InspectionTypeTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.InspectionType.FullName));
}

