using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public static class CountryFilter
    {
        public static IQueryable<Country> ByNumberCode(this IQueryable<Country> source, string numberCode, bool isIncludePassive = false)
        {
            if (numberCode.NullOrEmpty())
                throw new ArgumentException($"{nameof(numberCode)} cannot be null or empty string", nameof(numberCode));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Code.ToLower() == numberCode.ToLower());
        }

        public static IQueryable<Country> ByStringCode(this IQueryable<Country> source, string stringCode, bool isIncludePassive = false)
        {
            if (stringCode.NullOrEmpty())
                throw new ArgumentException($"{nameof(stringCode)} cannot be null or empty string", nameof(stringCode));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.TextCode.ToLower() == stringCode.ToLower());
        }
    }
}
