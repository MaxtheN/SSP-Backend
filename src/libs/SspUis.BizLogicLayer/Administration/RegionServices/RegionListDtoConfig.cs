using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;

namespace SspUis.BizLogicLayer.RegionServices
{
    public class RegionListDtoConfig : PerDtoConfig<RegionListDto, Region>
    {
        public override Action<IMappingExpression<Region, RegionListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x=>x.BankRegionId, x=>x.MapFrom(ent=>ent.BankRegionId))
                .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
                .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
                .ForMember(x => x.Country, x => x.MapFrom(ent => ent.Country.Translates.AsQueryable()
                    .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Country.FullName))
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName));
    }
}
