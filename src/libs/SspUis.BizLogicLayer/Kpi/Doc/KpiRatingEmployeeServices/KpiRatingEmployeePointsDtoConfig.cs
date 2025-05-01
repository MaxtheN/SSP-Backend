using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public class KpiRatingEmployeePointDtoConfig : PerDtoConfig<KpiRatingEmployeePointDto, KpiRatingEmployeePoint>
{
    public override Action<IMappingExpression<KpiRatingEmployeePoint, KpiRatingEmployeePointDto>> AlterReadMapping =>
        cfg => cfg
                   .ForMember(x => x.Indicator, x => x.MapFrom(ent => ent.Indicator.Translates.AsQueryable()
                    .FirstOrDefault(IndicatorTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Indicator.FullName));
}
