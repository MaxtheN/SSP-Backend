using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public static class VideoLessonListDtoSortFilter
    {
        public static IQueryable<VideoLessonListDto> SortFilter(this IQueryable<VideoLessonListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.Uri.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Theme.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Tag.ToLower().Contains(options.Search.ToLower()));
            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
