using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer;

public class IndicatorTableDtoConfig : PerDtoConfig<IndicatorTableDto, IndicatorTable>
{
	public override Action<IMappingExpression<IndicatorTable, IndicatorTableDto>> AlterReadMapping => base.AlterReadMapping;
}

