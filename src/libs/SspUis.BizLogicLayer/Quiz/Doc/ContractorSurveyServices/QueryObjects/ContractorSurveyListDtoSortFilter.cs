using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public static class ContractorSurveyListDtoSortFilter
{
    public static IQueryable<ContractorSurveyListDto> SortFilter(this IQueryable<ContractorSurveyListDto> query, ISortFilterOptions options)
    {
        if (options is ContractorSurveySortFilterOptions)
        {
            var _options = (ContractorSurveySortFilterOptions)options;
        }
        if (options.HasSearch())
            query = query.Where(a => a.Id.ToString().ToLower().Contains(options.Search.ToLower()));
        if (options.HasSort())
            query = query.OrderBy(options.SortBy, options.OrderType);
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
