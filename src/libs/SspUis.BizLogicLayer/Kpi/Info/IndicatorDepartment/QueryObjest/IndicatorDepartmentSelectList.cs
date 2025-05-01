using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class IndicatorDepartmentSelectList
{
	public static SelectList<int> AsSelectList(this IQueryable<IndicatorDepartment> query)
	{
		return new SelectList<int>(
			query
				.IsActive()
				.Select(a => new SelectListItem<int>
				{
					Value = a.Id,
					OrderCode = a.Code,
					Text = a.FullName
				})
				.OrderBy(a => a.Text)
			);
	}
}

