using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class UniteOfMeasureListDtoConfig : PerDtoConfig<UniteOfMeasureListDto, UniteOfMeasure>
{
	public override Action<IMappingExpression<UniteOfMeasure, UniteOfMeasureListDto>> AlterReadMapping => cfg => cfg
	   .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
			.FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
		;
}

