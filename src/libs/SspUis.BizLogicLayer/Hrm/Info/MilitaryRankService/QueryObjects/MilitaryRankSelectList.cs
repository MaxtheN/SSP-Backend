using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class MilitaryRankSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<MilitaryRank> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(x => new SelectListItem<int>
                {
                    Value = x.Id,
                    //OrderCode = x.OrderCode,
                    Text = x.Translates.AsQueryable().FirstOrDefault(MilitaryRankTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                })
                .OrderBy(x => x.Value)
                .ThenBy(x => x.Text)
            );
    }
}
