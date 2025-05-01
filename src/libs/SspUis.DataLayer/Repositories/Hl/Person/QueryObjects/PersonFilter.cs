using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class PersonFilter
    {
        public static IQueryable<Person> ByInn(this IQueryable<Person> source, string inn, bool isIncludePassive = false)
        {
            if (inn.NullOrEmpty())
                throw new ArgumentException($"{nameof(inn)} cannot be null or empty string", nameof(inn));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Inn.ToLower() == inn.ToLower());
        }

        public static IQueryable<Person> ByPinfl(this IQueryable<Person> source, string pinfl, bool isIncludePassive = false)
        {
            if (pinfl.NullOrEmpty())
                throw new ArgumentException($"{nameof(pinfl)} cannot be null or empty string", nameof(pinfl));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Pinfl.ToLower() == pinfl.ToLower());
        }
    }
}
