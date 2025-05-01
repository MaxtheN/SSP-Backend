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



public class IndicatorDepartmentTranslateDlDto : TranslateDto<IndicatorDepartmentTranslateDlDto,
	IndicatorDepartmentTranslate, TranslateColumn>,
	ILinkToEntity<IndicatorDepartmentTranslate>
{
}

public class IndicatorDepartmentTranslateDlDtoConfig : PerDtoConfig<IndicatorDepartmentTranslateDlDto, IndicatorDepartmentTranslate>
{
	public override Action<IMappingExpression<IndicatorDepartmentTranslate, IndicatorDepartmentTranslateDlDto>> AlterReadMapping => cfg => cfg
		.ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

	public override Action<IMappingExpression<IndicatorDepartmentTranslateDlDto, IndicatorDepartmentTranslate>> AlterSaveMapping =>
		cfg => cfg
			.ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}
