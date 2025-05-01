using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices;

public class PartisanshipTranslateDto : PartisanshipTranslateDlDto, ILinkToEntity<PartisanshipTranslate>
{
    public string Language { get; set; }
}
public class PartisanshipTranslateDtoConfig : PerDtoConfig<PartisanshipTranslateDto, PartisanshipTranslate>
{
    public override Action<IMappingExpression<PartisanshipTranslate, PartisanshipTranslateDto>> AlterReadMapping => cfg => cfg
        .IncludeBase<PartisanshipTranslate, PartisanshipTranslateDlDto>()
        .ForMember(x => x.Language, x => x.MapFrom(ent =>  ent.Language.FullName));
}
