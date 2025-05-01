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

public class KpiGratingIndicatorTableDtoConfig : PerDtoConfig<KpiGratingIndicatorTableDto, KpiGratingIndicatorTable>
{
    public override Action<IMappingExpression<KpiGratingIndicatorTable, KpiGratingIndicatorTableDto>> AlterReadMapping =>
        cfg => cfg
                   .ForMember(x => x.UniteOfMeasure, x => x.MapFrom(ent => /*ent.UniteOfMeasure.Translates.AsQueryable()*/
                    /*.FirstOrDefault(*//*UniteOfMeasureTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ??*/ ent.UniteOfMeasure.FullName));
                     
}

