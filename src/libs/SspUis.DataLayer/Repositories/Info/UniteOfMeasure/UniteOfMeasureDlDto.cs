using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;
public class UniteOfMeasureDlDto<TDto> :
	EntityDto<TDto, UniteOfMeasure>
	 where TDto : UniteOfMeasureDlDto<TDto>
{
	[LocalizedRequired]
	[LocalizedStringLength(9)]
	public string Code { get; set; } = null!;
	[LocalizedRequired]
	public int StateId { get; set; }
	[LocalizedRequired]
	[LocalizedStringLength(50)]
	public string OrderCode { get; set; }
	[LocalizedRequired]
	[LocalizedStringLength(250)]
	public string ShortName { get; set; } = null!;
	[LocalizedStringLength(500)]
	[LocalizedRequired]
	public string FullName { get; set; } = null!;
	public List<UniteOfMeasureTranslateDlDto> Translates { get; set; } = new();

	protected override Action<IMappingExpression<TDto, UniteOfMeasure>> AlterMapping => cfg => cfg
		.ForMember(x => x.Translates, x => x.Ignore());
	public override UniteOfMeasure CreateEntity()
	{
		var entity = base.CreateEntity();
		Translates.AddByUniqueFKTo(entity.Translates);
		return entity;
	}

	public override void UpdateEntity(UniteOfMeasure entity)
	{
		base.UpdateEntity(entity);
		Translates.ApplyChangesByUniqueFKTo(entity.Translates);
	}
}

