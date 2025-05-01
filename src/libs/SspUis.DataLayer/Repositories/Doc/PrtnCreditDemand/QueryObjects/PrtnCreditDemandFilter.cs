using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class PrtnCreditDemandFilter
    {
        public static IQueryable<PrtnCreditDemand> ByNumber(this IQueryable<PrtnCreditDemand> source, string number)
        {
            if (number.NullOrEmpty())
                throw new ArgumentException($"{nameof(number)} cannot be null or empty string", nameof(number));

            return source.Where(a => a.DocNumber.ToLower() == number.ToLower());
        }
    }
}
