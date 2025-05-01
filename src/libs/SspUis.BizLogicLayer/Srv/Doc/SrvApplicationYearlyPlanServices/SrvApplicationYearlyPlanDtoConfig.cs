using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class SrvApplicationYearlyPlanDtoConfig : PerDtoConfig<SrvApplicationYearlyPlanDto, SrvApplicationYearlyPlan>
{
    public override Action<IMappingExpression<SrvApplicationYearlyPlan, SrvApplicationYearlyPlanDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
              .ForMember(d => d.Region, c => c.MapFrom(e => e.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Region.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName));


}
