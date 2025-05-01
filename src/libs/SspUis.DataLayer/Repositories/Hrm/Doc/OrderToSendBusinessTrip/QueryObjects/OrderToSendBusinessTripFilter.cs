using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public static class OrderToSendBusinessTripFilter
    {
        public static IQueryable<OrderToSendBusinessTrip> ByDocNumber(this IQueryable<OrderToSendBusinessTrip> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));

            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
