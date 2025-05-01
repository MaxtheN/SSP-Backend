using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Quiz;

public static class QuestionnaireTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<QuestionnaireType> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
                .FirstOrDefault(QuestionnaireTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
