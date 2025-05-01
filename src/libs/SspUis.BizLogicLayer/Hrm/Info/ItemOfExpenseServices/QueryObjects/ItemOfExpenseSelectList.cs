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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.ItemOfExpenseServices
{
    public static class ItemOfExpenseSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ItemOfExpense> query, int? itemOfExpenseId = null)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Where(a => !itemOfExpenseId.HasValue || a.ParentId == itemOfExpenseId.Value)
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.Code,
                        Text = x.Translates.AsQueryable().FirstOrDefault(ItemOfExpenseTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
