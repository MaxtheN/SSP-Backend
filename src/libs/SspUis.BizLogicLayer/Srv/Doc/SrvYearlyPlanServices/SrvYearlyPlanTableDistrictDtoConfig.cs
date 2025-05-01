using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public class SrvYearlyPlanTableDistrictDtoConfig : PerDtoConfig<SrvYearlyPlanTableDistrictDto, SrvYearlyPlanTableDistrict>
{
    public override Action<IMappingExpression<SrvYearlyPlanTableDistrict, SrvYearlyPlanTableDistrictDto>> AlterReadMapping =>
        cfg => cfg
                   .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName));
}
