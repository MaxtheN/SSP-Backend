using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class IndicatorSelectList
{
	public static SelectList<int> AsSelectList(this IQueryable<Indicator> query)
	{
		return new SelectList<int>(
			query
				.IsActive()
				.Select(a => new SelectListItem<int>
				{
					Value = a.Id,
					OrderCode = a.OrderCode,
					Text = a.FullName
				})
				.OrderBy(a => a.OrderCode)
			);
	}
}

