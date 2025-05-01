using System;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.AppealTypeArriveServices;

public class AppealTypeArriveTranslateDto : AppealTypeArriveTranslateDlDto, ILinkToEntity<AppealTypeArriveTranslate>
{
    public string Language { get; set; }
}

public class AppealTypeArriveTranslateDtoConfig : PerDtoConfig<AppealTypeArriveTranslateDto, AppealTypeArriveTranslate>
{
    public override Action<IMappingExpression<AppealTypeArriveTranslate, AppealTypeArriveTranslateDto>> AlterReadMapping =>
        cfg => cfg
            .IncludeBase<AppealTypeArriveTranslate, AppealTypeArriveTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
