using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.QuestionnaireService;

public static class QuestionnaireSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<Questionnaire> query)
    {
        return new SelectList<long>(
            query
                .IsActive()
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    OrderCode = a.Title,
                    Text = a.Translates.AsQueryable().FirstOrDefault(QuestionnaireTranslate
                        .GetExpr(TranslateQuestionnaire.title, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? a.Title
                })
                .OrderBy(a => a.OrderCode)
                .ThenBy(a => a.Text)
            );
    }
}
