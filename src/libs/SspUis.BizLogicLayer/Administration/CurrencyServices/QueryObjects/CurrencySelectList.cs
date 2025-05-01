using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.CurrencyServices
{
    public static class CurrencySelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<Currency> query, int? langId)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new CurrencySelectListItemDto<int>
                    {
                        Value = x.Id,
                        OrderCode = x.OrderCode,
                        CurrencyCode = x.TextCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, langId ?? 3)).TranslateText ?? x.FullName
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
    public class CurrencySelectListItemDto<T> : SelectListItem<T>
    {
        public string CurrencyCode { get; set; }
    }
}
