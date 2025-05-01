using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public static class StateAwardSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<StateAward> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(x => new SelectListItem<int>
                {
                    Value = x.Id,
                    //OrderCode = x.OrderCode,
                    Text = x.Translates.AsQueryable().FirstOrDefault(StateAwardsTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                })
                .OrderBy(x => x.Value)
                .ThenBy(x => x.Text)
            );
    }
}
