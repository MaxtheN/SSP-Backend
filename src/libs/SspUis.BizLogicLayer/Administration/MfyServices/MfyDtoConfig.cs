using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.MfyServices
{
    public class MfyDtoConfig : PerDtoConfig<MfyDto, Mfy>
    {
        public override Action<IMappingExpression<Mfy, MfyDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName));
    }
}
