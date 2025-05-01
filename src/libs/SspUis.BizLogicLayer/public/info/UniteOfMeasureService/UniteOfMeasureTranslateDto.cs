using AutoMapper;
using GenericServices.Configuration;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer;

public class UniteOfMeasureTranslateDto : UniteOfMeasureTranslateDlDto, ILinkToEntity<UniteOfMeasureTranslate>
{
	public string Language { get; set; }
}

public class UniteOfMeasureTranslateDtoConfig : PerDtoConfig<UniteOfMeasureTranslateDto, UniteOfMeasureTranslate>
{
	public override Action<IMappingExpression<UniteOfMeasureTranslate, UniteOfMeasureTranslateDto>> AlterReadMapping =>
		cfg => cfg
			.IncludeBase<UniteOfMeasureTranslate, UniteOfMeasureTranslateDlDto>()
			.ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}

