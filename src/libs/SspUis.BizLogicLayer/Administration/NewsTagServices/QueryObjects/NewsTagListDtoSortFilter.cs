using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.NewsTagServices
{
    public static class NewsTagListDtoSortFilter
    {
        public static IQueryable<NewsTagListDto> SortFilter(this IQueryable<NewsTagListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.News.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
