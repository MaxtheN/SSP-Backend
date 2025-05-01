using System.Linq;
using SspUis.DataLayer.EfClasses.Quiz.Enum;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Quiz;

public static class AnswerTypeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<AnswerType> source)
    {
        return new SelectList<int>(source.Select(a => new SelectListItem<int>
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            Text = a.Translates.AsQueryable()
                .FirstOrDefault(AnswerTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
}
