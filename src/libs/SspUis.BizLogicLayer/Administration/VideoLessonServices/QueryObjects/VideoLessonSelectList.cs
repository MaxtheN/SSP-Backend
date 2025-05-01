using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public static class VideoLessonSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<VideoLesson> query, int? categoryId = null)
        {
            if (categoryId.HasValue)
                query = query.Where(a => a.CategoryId == categoryId.Value);

            return new SelectList<long>(query
                .IsActive()
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    OrderCode = a.OrderCode,
                    Text = a.Theme
                })
                .OrderBy(a => a.OrderCode)
                .ThenBy(a => a.Text));
        }
    }
}
