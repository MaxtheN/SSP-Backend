using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer
{
    public static class PositionSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Position> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new PositionAsSelectList
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
                        //PositionClassificationId = a.PositionClassificationId.Value,
                        //TariffScaleTypeId = a.TariffScaleTypeId,
                        //TariffScaleTypeName = a.TariffScaleType != null ? a.TariffScaleType.Translates.AsQueryable().FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.TariffScaleType.FullName : "",
                    })
                    .OrderBy(a => a.Text)
                );
        }

        public static SelectList<int> AsSelectList(this IEnumerable<OrganizationalStructurePosition> query)
        {
            return new SelectList<int>(
                query
                    .Select(a => new PositionAsSelectList
                    {
                        Value = a.PositionId,
                        Text = a.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Position.FullName,
                        PositionClassificationId = a.Position.PositionClassificationId.Value,
                        PositionTypeId = a.PositionTypeId,
                        PositionTypeName = a.PositionType != null ? (a.PositionType.Translates.AsQueryable().FirstOrDefault(PositionTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.PositionType.FullName) : "",
                        PositionCategoryId = a.PositionCategoryId,
                        PositionCategoryName = a.PositionCategory != null ? (a.PositionCategory.Translates.AsQueryable().FirstOrDefault(PositionCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.PositionCategory.FullName) : "",
                        TariffScaleTypeId = a.TariffScaleTypeId,
                        TariffScaleTypeName = a.TariffScaleType != null ? a.TariffScaleType.Translates.AsQueryable().FirstOrDefault(TariffScaleTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.TariffScaleType.FullName : "",
                        TariffScaleId = a.TariffScaleId,
                        TariffScaleName = a.TariffScale != null ? a.TariffScale.Translates.AsQueryable().FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.TariffScale.FullName : "",
                        RankId = a.RankId,
                        RankCode = a.Rank?.RankCode ?? "",
                        Amount = a.Amount,
                        StaffingQuantity = a.StaffingQuantity,
                    })
                    .OrderBy(a => a.Text)
                );
        }
        public class PositionAsSelectList : SelectListItem<int>
        {
            public int PositionClassificationId { get; set; }
            public int? PositionTypeId { get; set; }
            public string? PositionTypeName { get; set; }
            public int? PositionCategoryId { get; set; }
            public string? PositionCategoryName { get; set; }
            public int? TariffScaleTypeId { get; set; }
            public string TariffScaleTypeName { get; set; }
            public int? TariffScaleId { get; set; }
            public string TariffScaleName { get; set; }
            public int? RankId { get; set; }
            public string RankCode { get; set; }
            public decimal? Amount { get; set; }
            public decimal? StaffingQuantity { get; set; }
        }
    }
}
