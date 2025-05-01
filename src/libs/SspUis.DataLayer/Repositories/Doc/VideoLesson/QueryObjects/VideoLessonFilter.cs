using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class VideoLessonFilter
    {
        public static IQueryable<VideoLesson> ByNumber(this IQueryable<VideoLesson> source, string number, bool isIncludePassive = false)
        {
            if (number.NullOrEmpty())
                throw new ArgumentException($"{nameof(number)} cannot be null or empty string", nameof(number));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Number.ToLower() == number.ToLower());
        }
    }
}
