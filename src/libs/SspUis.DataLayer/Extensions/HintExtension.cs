using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer
{
    public static class HintExtension
    {
        public static IQueryable<T> WithHint<T>(this IQueryable<T> set, string hint) where T : class
        {
            HintInterceptor.HintValue = hint;
            return set;
        }
        public static IQueryable<T> ForUpdate<T>(this IQueryable<T> set) where T : class
        {
            return set.WithHint("FOR UPDATE");
        }
        public static void Lock<TId, T>(this IQueryable<T> set, params TId[] ids)
            where T : class, IHaveIdProp<TId>
        {
            if (ids.Length > 0)
                set.Where(a => ids.Contains(a.Id)).Select(a => new { a.Id }).ForUpdate().ToArray();
        }

        public static void Lock(this IQueryable<NumberTemplate> set, string document)
        {
            set.Where(a => a.Document == document).ForUpdate().Select(a => a.Id).ToArray();
        }
    }
}
