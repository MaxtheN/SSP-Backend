using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class QuestionGroupTranslateDlDto :
    TranslateDto<QuestionGroupTranslateDlDto, QuestionGroupTranslate, TranslateQuestionGroup>,
    ILinkToEntity<QuestionGroupTranslate>
{

}

public class QuestionGroupTranslateDlDtoConfig : PerDtoConfig<QuestionGroupTranslateDlDto, QuestionGroupTranslate>
{
    public override Action<IMappingExpression<QuestionGroupTranslate, QuestionGroupTranslateDlDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateQuestionGroup>()));

    public override Action<IMappingExpression<QuestionGroupTranslateDlDto, QuestionGroupTranslate>> AlterSaveMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}

public enum TranslateQuestionGroup
{
    title
}