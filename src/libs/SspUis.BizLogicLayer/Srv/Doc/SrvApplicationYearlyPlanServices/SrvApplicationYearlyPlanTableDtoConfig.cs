using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanTableDtoConfig : PerDtoConfig<SrvApplicationYearlyPlanTableDto, SrvApplicationYearlyPlanTable>
{
    public override Action<IMappingExpression<SrvApplicationYearlyPlanTable, SrvApplicationYearlyPlanTableDto>> AlterReadMapping =>
        cfg => cfg
                   .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName));
}
