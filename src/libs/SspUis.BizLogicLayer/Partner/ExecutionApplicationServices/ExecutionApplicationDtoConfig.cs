using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices
{
    public class ExecutionApplicationDtoConfig : PerDtoConfig<ExecutionApplicationDto, ExecutionApplication>
    {
        public override Action<IMappingExpression<ExecutionApplication, ExecutionApplicationDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(d => d.Region, c => c.MapFrom(e => e.Region.Translates.AsQueryable()
               .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.Region.FullName))
            .ForMember(d => d.District, c => c.MapFrom(e => e.District.Translates.AsQueryable()
               .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? e.District.FullName))
            ;
    }
}
