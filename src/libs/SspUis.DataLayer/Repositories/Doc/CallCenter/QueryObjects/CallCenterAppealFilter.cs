using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories
{
    public static class CallCenterAppealFilter
    {
        public static IQueryable<CallCenterAppeal> ByDocNumber(this IQueryable<CallCenterAppeal> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
