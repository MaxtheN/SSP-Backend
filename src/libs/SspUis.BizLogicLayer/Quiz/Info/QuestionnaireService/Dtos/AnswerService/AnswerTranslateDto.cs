using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace IhmaInv.BizLogicLayer.AnswerServices;

public class AnswerTranslateDto : AnswerTranslateDlDto, ILinkToEntity<AnswerTranslate>
{
    public string Language { get; set; }
}

public class AnswerTranslateDtoConfig : PerDtoConfig<AnswerTranslateDto, AnswerTranslate>
{
    public override Action<IMappingExpression<AnswerTranslate, AnswerTranslateDto>> AlterReadMapping =>
        cfg => cfg
            .IncludeBase<AnswerTranslate, AnswerTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
