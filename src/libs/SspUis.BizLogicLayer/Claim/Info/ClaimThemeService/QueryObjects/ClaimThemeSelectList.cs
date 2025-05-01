using WEBASE;
using WEBASE.Models;
using WEBASE.DependencyInjection;
using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim.ClaimThemeServices
{
    public static class ClaimThemeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ClaimTheme> query, int? langId)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(ClaimThemeTranslate.GetExpr(TranslateColumn.full_name, langId ?? 3)).TranslateText ?? a.FullName
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
