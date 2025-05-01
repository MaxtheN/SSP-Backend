using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class QuestionnaireTranslateDlDto :
    TranslateDto<QuestionnaireTranslateDlDto, QuestionnaireTranslate, TranslateQuestionnaire>,
    ILinkToEntity<QuestionnaireTranslate>
{

}

public class QuestionnaireTranslateDlDtoConfig : PerDtoConfig<QuestionnaireTranslateDlDto, QuestionnaireTranslate>
{
    public override Action<IMappingExpression<QuestionnaireTranslate, QuestionnaireTranslateDlDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateQuestionGroup>()));

    public override Action<IMappingExpression<QuestionnaireTranslateDlDto, QuestionnaireTranslate>> AlterSaveMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}

public enum TranslateQuestionnaire
{
    title
}