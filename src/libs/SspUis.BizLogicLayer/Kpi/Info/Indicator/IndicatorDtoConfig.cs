using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer;
public class IndicatorDtoConfig : PerDtoConfig<IndicatorDto, Indicator>
{
	public override Action<IMappingExpression<Indicator, IndicatorDto>> AlterReadMapping => cgf => cgf
			.ForMember(x => x.State, x => x.MapFrom(ent =>
				ent.State.Translates.AsQueryable().FirstOrDefault(
					StateTranslate.GetExpr(
						TranslateColumn.full_name,
						ServiceProvider.CultureHelper.CurrentCulture.Id))
				.TranslateText ?? ent.State.FullName))
		   .ForMember(x => x.Department, x => x.MapFrom(ent =>
				ent.Department.FullName))
	;
}

