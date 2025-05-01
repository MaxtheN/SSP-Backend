using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class IndicatorDepartmentDlDto<TDto> :
	EntityDto<TDto, IndicatorDepartment> where TDto :
	IndicatorDepartmentDlDto<TDto>
{
	[LocalizedRequired]
	public string Code { get; set; } = null;
	[LocalizedRequired]
	public int StateId { get; }
	[LocalizedRequired]
	public string OrderCode { get; set; }
	[LocalizedRequired]
	public string ShortName { get; set; } = null;
	[LocalizedRequired]
	public string FullName { get; set; } = null;
	public List<IndicatorDepartmentTranslateDlDto> Translates { get; set; } = new();
	protected override Action<IMappingExpression<TDto, IndicatorDepartment>> AlterMapping => cfg => cfg
			.ForMember(x => x.Translates, x => x.Ignore());

	public override IndicatorDepartment CreateEntity()
	{
		var res = base.CreateEntity();
		res.StateId = StateIdConst.ACTIVE;
		Translates.AddByUniqueFKTo(res.Translates);
		res.CreatedAt = DateTime.Now;
		return res;
	}
	public override void UpdateEntity(IndicatorDepartment entity)
	{
		base.UpdateEntity(entity);
		Translates.ApplyChangesByUniqueFKTo(entity.Translates);
	}
}

