using AutoMapper;
using GenericServices.Configuration;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories;


public class UniteOfMeasureTranslateDlDto : TranslateDto<UniteOfMeasureTranslateDlDto, UniteOfMeasureTranslate, TranslateColumn>, ILinkToEntity<UniteOfMeasureTranslate>
{
}

public class UniteOfMeasureTranslateDlDtoConfig : PerDtoConfig<UniteOfMeasureTranslateDlDto, UniteOfMeasureTranslate>
{
	public override Action<IMappingExpression<UniteOfMeasureTranslate, UniteOfMeasureTranslateDlDto>> AlterReadMapping => cfg => cfg
		.ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

	public override Action<IMappingExpression<UniteOfMeasureTranslateDlDto, UniteOfMeasureTranslate>> AlterSaveMapping =>
		cfg => cfg
			.ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}
