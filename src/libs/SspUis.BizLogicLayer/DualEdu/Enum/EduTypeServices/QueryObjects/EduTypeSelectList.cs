using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DualEdu.EnumEduTypeServices;

public static class EduTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<EduType> query)
    {
        return new SelectList<int>(
            query.Select(x => new SelectListItem<int>
            {
                Value = x.Id,
                OrderCode = x.Code,
                Text = x.Translates.AsQueryable().FirstOrDefault(EduTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
            })
            .OrderBy(x => x.OrderCode)
            .ThenBy(x => x.Text)
            );
    }
}