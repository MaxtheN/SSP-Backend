using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class QuestionTranslateDlDto :
    TranslateDto<QuestionTranslateDlDto, QuestionTranslate, TranslateQuestion>,
    ILinkToEntity<QuestionTranslate>
{

}

public class QuestionTranslateDlDtoConfig : PerDtoConfig<QuestionTranslateDlDto, QuestionTranslate>
{
    public override Action<IMappingExpression<QuestionTranslate, QuestionTranslateDlDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateQuestion>()));

    public override Action<IMappingExpression<QuestionTranslateDlDto, QuestionTranslate>> AlterSaveMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}

public enum TranslateQuestion
{
    question_text
}