using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class BankCodeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<BankCode> source)
        {
            return new SelectList<int>(source
                .Include(a => a.Translates)
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.Translates.AsQueryable().FirstOrDefault(BankCodeTranslate.GetExpr(TranslateColumn.full_name)).TranslateText ?? a.FullName,
                }));
        }
    }
}
