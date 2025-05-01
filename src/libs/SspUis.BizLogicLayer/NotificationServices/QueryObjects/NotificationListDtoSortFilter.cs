using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.NotificationServices
{
    public static class NotificationListDtoSortFilter
    {
        public static IQueryable<NotificationListDto> SortFilter(this IQueryable<NotificationListDto> query, NotificationSortFilterPageOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.Title.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Content.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
