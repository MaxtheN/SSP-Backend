using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Kpi;

public class KpiPlanEmployeeIndicatorCreateDtoConfig : PerDtoConfig<KpiPlanEmployeeIndicatorCreateDto, KpiPlanEmployeeIndicatorCreate>

{
    public override Action<IMappingExpression<KpiPlanEmployeeIndicatorCreate, KpiPlanEmployeeIndicatorCreateDto>> AlterReadMapping =>
    cfg => cfg
        .ForMember(d => d.Indicator, c => c.MapFrom(e => e.Indicator.Translates.AsQueryable()
            .FirstOrDefault(IndicatorTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Indicator.FullName))
    ;
    

}
