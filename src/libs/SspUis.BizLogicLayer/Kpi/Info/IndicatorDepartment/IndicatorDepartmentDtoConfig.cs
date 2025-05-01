using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer;
public class IndicatorDepartmentDtoConfig : PerDtoConfig<IndicatorDepartmentDto, IndicatorDepartment>
{
	public override Action<IMappingExpression<IndicatorDepartment, IndicatorDepartmentDto>> AlterReadMapping => cgf => cgf
			.ForMember(x => x.State, x => x.MapFrom(ent =>
				ent.State.Translates.AsQueryable().FirstOrDefault(
					StateTranslate.GetExpr(
						TranslateColumn.full_name,
						ServiceProvider.CultureHelper.CurrentCulture.Id))
				.TranslateText ?? ent.State.FullName))
	;
}

