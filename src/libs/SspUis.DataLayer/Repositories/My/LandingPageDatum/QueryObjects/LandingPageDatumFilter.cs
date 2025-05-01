using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class LandingPageDatumFilter
    {
        public static IQueryable<LandingPageDatum> ByNumberCode(this IQueryable<LandingPageDatum> source, string numberCode, bool isIncludePassive = false)
        {
            if (numberCode.NullOrEmpty())
                throw new ArgumentException($"{nameof(numberCode)} cannot be null or empty string", nameof(numberCode));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.OrderCode.ToLower() == numberCode.ToLower());
        }
    }
}
