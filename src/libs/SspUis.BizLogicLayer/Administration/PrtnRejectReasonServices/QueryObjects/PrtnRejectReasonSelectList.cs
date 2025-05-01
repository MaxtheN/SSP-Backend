using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.PrtnRejectReasonServices
{
    public static class PrtnRejectReasonSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<PrtnRejectReason> query)
        {
            return new SelectList<int>(
                query
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(PrtnRejectReasonTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
