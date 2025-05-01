using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices
{
    public static class TariffScaleSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<TariffScale> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.Code,
                        Text = x.Translates.AsQueryable().FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                );
        }
        public static SelectList<int> AsSelectList(this IQueryable<TariffScale> query, int typeId)
        {
            return new SelectList<int>(
                query
                    .Include(a => a.TariffScaleType)
                    .IsActive()
                    .Where(a => a.TariffScaleTypeId == typeId)
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        Text = a.Translates.AsQueryable().FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
                        OrderCode = a.Code
                    })
                    .OrderBy(a => a.OrderCode)
                );
        }

        public static SelectList<int> AsTableSelectList(this IQueryable<TariffScale> query, int id)
        {
            return new SelectList<int>(
                query
                    .Include(a => a.Tables)
                    .IsActive()
                    .FirstOrDefault(a => a.Id == id)?.Tables
                    .Select(a => new NumberOrderCodeSelectItem
                    {
                        Value = a.Id,
                        Text = a.RankCode,
                        OrderCode = a.OrderCode.Value
                    }).OrderBy(a => a.OrderCode)
                );
        }
        public class NumberOrderCodeSelectItem : SelectListItem<int>
        {
            new public int OrderCode { get; set; }
        }
    }
}
