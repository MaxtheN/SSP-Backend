using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public class ContractorSurveyTableDtoConfig : PerDtoConfig<ContractorSurveyTableDto, ContractorSurveyTable>
{
    public override Action<IMappingExpression<ContractorSurveyTable, ContractorSurveyTableDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.GroupId, x => x.MapFrom(ent => ent.QuestionnaireGroup.GroupId))
        .ForMember(x => x.QuestionId, x => x.MapFrom(ent => ent.QuestionnaireQuestion.QuestionId))
        .ForMember(x => x.AnswerId, x => x.MapFrom(ent => ent.QuestionnaireAnswerId == null ? 0 : ent.QuestionnaireAnswer.AnswerId))
            .ForMember(x => x.ContractorOrderClientName,
                x => x.MapFrom(ent => ent.TextAnswer));
}
