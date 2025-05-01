using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace IhmaInv.BizLogicLayer.QuestionGroupService;

public class QuestionGroupTranslateDto : QuestionGroupTranslateDlDto, ILinkToEntity<QuestionGroupTranslate>
{
    public string Language { get; set; }
}

public class QuestionGroupTranslateDtoConfig : PerDtoConfig<QuestionGroupTranslateDto, QuestionGroupTranslate>
{
    public override Action<IMappingExpression<QuestionGroupTranslate, QuestionGroupTranslateDto>> AlterReadMapping =>
        cfg => cfg
            .IncludeBase<QuestionGroupTranslate, QuestionGroupTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
