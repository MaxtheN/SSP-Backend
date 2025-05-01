using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class PrtnContractFilter
    {
        public static IQueryable<PrtnContract> ByNumber(this IQueryable<PrtnContract> source, string number)
        {
            if (number.NullOrEmpty())
                throw new ArgumentException($"{nameof(number)} cannot be null or empty string", nameof(number));

            return source.Where(a => a.DocNumber.ToLower() == number.ToLower());
        }
    }
}
