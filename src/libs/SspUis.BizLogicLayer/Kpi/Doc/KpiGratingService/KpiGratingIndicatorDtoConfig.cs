using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class KpiGratingIndicatorDtoConfig : PerDtoConfig<KpiGratingIndicatorDto, KpiGratingIndicator>
{
    public override Action<IMappingExpression<KpiGratingIndicator, KpiGratingIndicatorDto>> AlterReadMapping =>
        cfg => cfg
                       .ForMember(x => x.Indicator, x => x.MapFrom(ent => ent.Indicator.Translates.AsQueryable()
                       .FirstOrDefault(IndicatorTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Indicator.FullName))
                     .ForMember(x => x.Tables, x => x.MapFrom(ent => ent.Tables));
}
