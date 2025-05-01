using System;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.AppealDescriptionServices;

public class AppealDescriptionTranslateDto : AppealDescriptionTranslateDlDto, ILinkToEntity<AppealDescriptionTranslate>
{
    public string Language { get; set; }
}

public class AppealDescriptionTranslateDtoConfig : PerDtoConfig<AppealDescriptionTranslateDto, AppealDescriptionTranslate>
{
    public override Action<IMappingExpression<AppealDescriptionTranslate, AppealDescriptionTranslateDto>> AlterReadMapping =>
        cfg => cfg
            .IncludeBase<AppealDescriptionTranslate, AppealDescriptionTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
