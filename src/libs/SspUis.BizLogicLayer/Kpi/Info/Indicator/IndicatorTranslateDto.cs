using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;
public class IndicatorTranslateDto: 
	IndicatorTranslateDlDto, 
	ILinkToEntity<IndicatorTranslate>
{
	public string Language { get; set; }
}

public class IndicatorTranslateDtoConfig : PerDtoConfig<IndicatorTranslateDto, IndicatorTranslate>
{
	public override Action<IMappingExpression<IndicatorTranslate, IndicatorTranslateDto>> AlterReadMapping =>
		cfg => cfg
			.IncludeBase<IndicatorTranslate, IndicatorTranslateDlDto>()
			.ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}

