using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Memship;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipYearlyPlanTableDtoConfig : PerDtoConfig<MemshipYearlyPlanTableDto, MemshipYearlyPlanTable>
{
    public override Action<IMappingExpression<MemshipYearlyPlanTable, MemshipYearlyPlanTableDto>> AlterReadMapping =>
        cfg => cfg
                   .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName));
}
