using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace IhmaInv.BizLogicLayer.QuestionServices;

public class QuestionnaireTranslateDto : QuestionnaireTranslateDlDto, ILinkToEntity<QuestionnaireTranslate>
{
    public string Language { get; set; }
}

public class QuestionnaireTranslateDtoConfig : PerDtoConfig<QuestionnaireTranslateDto, QuestionnaireTranslate>
{
    public override Action<IMappingExpression<QuestionnaireTranslate, QuestionnaireTranslateDto>> AlterReadMapping =>
        cfg => cfg
            .IncludeBase<QuestionnaireTranslate, QuestionnaireTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}