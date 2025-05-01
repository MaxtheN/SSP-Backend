using SspUis.BizLogicLayer.Hrm;
using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;
public static class IndicatorDepartmentListDtoSortFilter
{
	public static IQueryable<IndicatorDepartmentListDto> SortFilter(this IQueryable<IndicatorDepartmentListDto> query, ISortFilterOptions options)
	{
		if (options.HasSearch())
			query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
									 a.FullName.ToLower().Contains(options.Search.ToLower()) ||
									 a.Code.ToLower().Contains(options.Search.ToLower()));

		if (options.HasSort())
			query = query.OrderBy($"{options.SortBy} {options.OrderType}");
		else
			query = query.OrderByDescending(a => a.Id);

		return query;
	}
}

