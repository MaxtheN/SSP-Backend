using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public static class ContractorFilter
    {
        public static IQueryable<Contractor> ByInn(this IQueryable<Contractor> source, string inn, bool isIncludePassive = false)
        {
            if (inn.NullOrEmpty())
                throw new ArgumentException($"{nameof(inn)} cannot be null or empty string", nameof(inn));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Inn.ToLower() == inn.ToLower());
        }

        public static IQueryable<Contractor> ByPinfl(this IQueryable<Contractor> source, string pinfl, bool isIncludePassive = false)
        {
            if (pinfl.NullOrEmpty())
                throw new ArgumentException($"{nameof(pinfl)} cannot be null or empty string", nameof(pinfl));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Pinfl.ToLower() == pinfl.ToLower());
        }
    }
}
