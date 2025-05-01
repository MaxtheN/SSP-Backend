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
using System.Linq.Dynamic.Core;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.PositionServices
{
    public class PositionListDtoConfig : PerDtoConfig<PositionListDto, Position>
    {
        public override Action<IMappingExpression<Position, PositionListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                    .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
                .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                    .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
                .ForMember(x => x.PositionClassification, x => x.MapFrom(ent => ent.PositionClassification.Translates.AsQueryable()
                    .FirstOrDefault(PositionClassificationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionClassification.FullName))
                .ForMember(x => x.TariffScaleType, x => x.MapFrom(ent => ent.TariffScaleType.Translates.AsQueryable()
                    .FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScaleType.FullName))
                .ForMember(x => x.StaffTypeBasicTariff, x => x.MapFrom(ent => ent.StaffTypeBasicTariff.Translates.AsQueryable()
                    .FirstOrDefault(StaffTypeBasicTariffTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.StaffTypeBasicTariff.FullName))
            .ForMember(x => x.PositionCategory, x => x.MapFrom(ent => ent.PositionCategory.Translates.AsQueryable()
                    .FirstOrDefault(PositionCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionCategory.FullName))
                ;
    }
}
