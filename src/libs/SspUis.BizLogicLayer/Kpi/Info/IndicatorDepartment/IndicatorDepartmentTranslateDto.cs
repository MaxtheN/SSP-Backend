using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;
public class IndicatorDepartmentTranslateDto: 
	IndicatorDepartmentTranslateDlDto, 
	ILinkToEntity<IndicatorDepartmentTranslate>
{
	public string Language { get; set; }
}

public class IndicatorDepartmentTranslateDtoConfig : PerDtoConfig<IndicatorDepartmentTranslateDto, IndicatorDepartmentTranslate>
{
	public override Action<IMappingExpression<IndicatorDepartmentTranslate, IndicatorDepartmentTranslateDto>> AlterReadMapping =>
		cfg => cfg
			.IncludeBase<IndicatorDepartmentTranslate, IndicatorDepartmentTranslateDlDto>()
			.ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}

