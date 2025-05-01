using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer
{
    public static class CandidatesConfirmationListDtoSortFilter
    {
        public static IQueryable<CandidatesConfirmationListDto> SortFilter(this IQueryable<CandidatesConfirmationListDto> query, CandidatesConfirmationSortFilterPageOption options)
        {
            if(options.StatusId.HasValue)
                query = query.Where(x => x.StatusId ==  options.StatusId);

            if (options.DepartmentId.HasValue)
                query = query.Where(x => x.DepartmentId == options.DepartmentId);

            if (options.PositionId.HasValue)
                query = query.Where(x => x.PositionId == options.PositionId);

            if (options.HasSearch())
                query = query.Where(a => ("" + a.DocNumber.ToLower()).Contains(options.Search.ToLower()) ||
                                        a.Status.ToLower().Contains(options.Search.ToLower()) ||
                                        a.DocContent.ToLower().Contains(options.Search.ToLower()) ||
                                        a.GeneralConclusion.ToLower().Contains(options.Search.ToLower()))
                                .AsQueryable();

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
