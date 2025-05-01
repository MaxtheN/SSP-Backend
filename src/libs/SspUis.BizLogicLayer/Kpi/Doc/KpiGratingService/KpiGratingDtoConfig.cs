using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class KpiGratingDtoConfig : PerDtoConfig<KpiGratingDto, KpiGrating>
{
    public override Action<IMappingExpression<KpiGrating, KpiGratingDto>> AlterReadMapping =>
       cfg => cfg
           .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
               .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
             .ForMember(x => x.Indicators, x => x.MapFrom(ent => ent.Indicators))
           .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
               .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName));


}
