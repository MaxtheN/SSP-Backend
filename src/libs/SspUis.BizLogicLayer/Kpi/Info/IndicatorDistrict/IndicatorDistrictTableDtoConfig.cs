using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer
{
    public class IndicatorDistrictTableDtoConfig : PerDtoConfig<IndicatorDistrictTableDto, IndicatorDistrictTable>
    {
        public override Action<IMappingExpression<IndicatorDistrictTable, IndicatorDistrictTableDto>> AlterReadMapping => base.AlterReadMapping;
    }
}
