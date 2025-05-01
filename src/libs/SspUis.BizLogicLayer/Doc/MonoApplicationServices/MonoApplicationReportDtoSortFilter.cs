using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Doc.MonoApplicationServices
{
	public static class MonoApplicationReportDtoSortFilter
	{
		public static IQueryable<MonoApplication> SortFilter(this IQueryable<MonoApplication> query, MonoApplicationReportSortFilter options)
		{
			if (options.StartDate != null && options.EndDate != null)
				query = query.Where(q => q.CreatedAt >= options.StartDate && q.CreatedAt <= options.EndDate);

			if (options.StartDate == null && options.EndDate != null)
				query = query.Where(q => q.CreatedAt <= options.EndDate);

			if (options.StartDate != null && options.EndDate == null)
				query = query.Where(q => q.CreatedAt >= options.StartDate);

			return query;
		}
	}
}
