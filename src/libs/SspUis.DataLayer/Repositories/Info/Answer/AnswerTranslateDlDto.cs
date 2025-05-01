using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class AnswerTranslateDlDto :
    TranslateDto<AnswerTranslateDlDto, AnswerTranslate, TranslateAnswer>,
    ILinkToEntity<AnswerTranslate>
{ }

public class AnswerTranslateDlDtoConfig : PerDtoConfig<AnswerTranslateDlDto, AnswerTranslate>
{
    public override Action<IMappingExpression<AnswerTranslate, AnswerTranslateDlDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateAnswer>()));

    public override Action<IMappingExpression<AnswerTranslateDlDto, AnswerTranslate>> AlterSaveMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}

public enum TranslateAnswer
{
    answer_text
}