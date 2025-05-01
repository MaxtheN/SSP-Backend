using SspUis.BizLogicLayer.Arbitration.Doc.ArbitrationDiscussionServices.QueryObjects;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer;

public static class ArbitrationDiscussionListDtoSortFilter
{
	public static IQueryable<ArbitrationDiscussionListDto> SortFilter(
		this IQueryable<ArbitrationDiscussionListDto> query,
		ArbitrationDiscussionSortFilterOptions options)
	{
		if (options.FromDocDate.HasValue)
			query = query.Where(a => a.DocOn >= options.FromDocDate);

		if (options.ToDocDate.HasValue)
			query = query.Where(a => a.DocOn <= options.ToDocDate);

		if (options.HasSearch())
			query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower()));

		if (options.HasSort())
			query = query.OrderBy($"{options.SortBy} {options.OrderType}");
		else
			query = query.OrderByDescending(a => a.Id);

		return query;
	}
}
