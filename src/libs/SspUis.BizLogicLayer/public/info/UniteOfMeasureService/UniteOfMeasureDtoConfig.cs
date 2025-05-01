using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer;
public class UniteOfMeasureDtoConfig : PerDtoConfig<UniteOfMeasureDto, UniteOfMeasure>
{
	public override Action<IMappingExpression<UniteOfMeasure, UniteOfMeasureDto>> AlterReadMapping => cgf => cgf
			 .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.FullName))
	;
}

