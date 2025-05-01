using System;
using System.Linq;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Info.OrganizationalStructure;

namespace SspUis.BizLogicLayer.Info.OrganizationalStructureServices
{
    public class OrganizationalStructurePositionDto : OrganizationalStructurePositionDlDto, ILinkToEntity<OrganizationalStructurePosition>
    {
        public string Position { get; set; }
        public string PositionType { get; set; } = null!;
        public string PositionCategory { get; set; } = null!;
        public string TariffScaleType { get; set; } = null!;
        public string TariffScale { get; set; } = null!;
        public string? RankCode { get; set; }
        public string? SchoolGroupContingent { get; set; }
    }
    public class OrganizationalStructurePositionConfigDto : PerDtoConfig<OrganizationalStructurePositionDto, OrganizationalStructurePosition>
    {
        public override Action<IMappingExpression<OrganizationalStructurePosition, OrganizationalStructurePositionDto>> AlterReadMapping =>
            cfg => cfg.ForMember(x => x.Position, env => env.MapFrom(x => x.Position.ShortName))
             .ForMember(x => x.PositionType, x => x.MapFrom(ent => ent.PositionType.Translates.AsQueryable().FirstOrDefault(PositionTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionType.FullName))
                .ForMember(x => x.PositionCategory, x => x.MapFrom(ent => ent.PositionCategory.Translates.AsQueryable().FirstOrDefault(PositionCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PositionCategory.FullName))
                .ForMember(x => x.TariffScaleType, x => x.MapFrom(ent => ent.TariffScaleType.Translates.AsQueryable().FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScaleType.FullName))
                .ForMember(x => x.RankCode, x => x.MapFrom(ent => ent.Rank != null ? ent.Rank.RankCode : null))
                .ForMember(x => x.TariffScale, x => x.MapFrom(ent => ent.TariffScale != null ? (ent.TariffScale.Translates.AsQueryable().FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.TariffScale.FullName) : ""))
              ;
    }
}
