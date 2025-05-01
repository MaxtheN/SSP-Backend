using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace IhmaInv.BizLogicLayer.QuestionServices;

public class QuestionTranslateDto : QuestionTranslateDlDto, ILinkToEntity<QuestionTranslate>
{
    public string Language { get; set; }
}

public class QuestionTranslateDtoConfig : PerDtoConfig<QuestionTranslateDto, QuestionTranslate>
{
    public override Action<IMappingExpression<QuestionTranslate, QuestionTranslateDto>> AlterReadMapping =>
        cfg => cfg
            .IncludeBase<QuestionTranslate, QuestionTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
