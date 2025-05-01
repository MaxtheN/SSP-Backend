using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanTableRegionDtoConfig : PerDtoConfig<SrvYearlyPlanTableRegionDto, SrvYearlyPlanTableRegion>
{
    public override Action<IMappingExpression<SrvYearlyPlanTableRegion, SrvYearlyPlanTableRegionDto>> AlterReadMapping =>
        cfg => cfg
                   .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                     .ForMember(x => x.Districts, x => x.MapFrom(ent => ent.Districts));
}
