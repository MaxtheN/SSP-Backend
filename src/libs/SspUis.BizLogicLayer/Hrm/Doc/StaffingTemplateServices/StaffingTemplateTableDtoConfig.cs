using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateTableDtoConfig : PerDtoConfig<StaffingTemplateTableDto, StaffingTemplateTable>
    {
        public override Action<IMappingExpression<StaffingTemplateTable, StaffingTemplateTableDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable()
                    .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.FullName))
                .ForMember(x => x.TariffScaleType, x => x.MapFrom(ent => ent.TariffScaleType.Translates.AsQueryable()
                    .FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScaleType.FullName))
                .ForMember(x => x.TariffScale, x => x.MapFrom(ent => ent.TariffScale.Translates.AsQueryable()
                    .FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScale.FullName))
                .ForMember(x => x.TariffScaleTable,x => x.MapFrom(ent => ent.TariffScaleTable.RankCode))
            ;
    }
}
