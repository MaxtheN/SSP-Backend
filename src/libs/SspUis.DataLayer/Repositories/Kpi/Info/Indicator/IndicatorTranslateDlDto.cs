using AutoMapper;
using GenericServices.Configuration;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using WEBASE;

namespace SspUis.DataLayer.Repositories;



public class IndicatorTranslateDlDto : TranslateDto<IndicatorTranslateDlDto,
	IndicatorTranslate, TranslateColumn>,
	ILinkToEntity<IndicatorTranslate>
{
}

public class IndicatorTranslateDlDtoConfig : PerDtoConfig<IndicatorTranslateDlDto, IndicatorTranslate>
{
	public override Action<IMappingExpression<IndicatorTranslate, IndicatorTranslateDlDto>> AlterReadMapping => cfg => cfg
		.ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

	public override Action<IMappingExpression<IndicatorTranslateDlDto, IndicatorTranslate>> AlterSaveMapping =>
		cfg => cfg
			.ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}
