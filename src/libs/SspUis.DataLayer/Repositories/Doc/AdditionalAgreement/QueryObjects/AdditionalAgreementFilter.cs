using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class AdditionalAgreementFilter
    {
        public static IQueryable<AdditionalAgreement> ByNumber(this IQueryable<AdditionalAgreement> source, string number, bool isIncludePassive = false)
        {
            if (number.NullOrEmpty())
                throw new ArgumentException($"{nameof(number)} cannot be null or empty string", nameof(number));

            return source.Where(a => a.DocNumber.ToLower() == number.ToLower());
        }
    }
}
