using System.Linq;
using System.Linq.Dynamic.Core;
namespace SspUis.BizLogicLayer.QuestionnaireService;
public static class QuestionnaireListSortFilterOptions
{
    public static IQueryable<QuestionnaireListDto> SortFilter(
        this IQueryable<QuestionnaireListDto> query,
        QuestionnaireSortFilterOptionsDto options)
    {
        if (options.HasSearch())
            query = query.Where(a =>
                   a.Title.ToLower().Contains(options.Search.ToLower())
                || a.Details.ToLower().Contains(options.Search.ToLower()));

        return options.HasSearch()
            ? query.OrderBy($"{options.SortBy} {options.OrderType}")
            : query.OrderByDescending(a => a.Id); ;
    }
}