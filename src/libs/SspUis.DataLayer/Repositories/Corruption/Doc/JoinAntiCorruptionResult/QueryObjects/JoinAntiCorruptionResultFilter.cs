using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Corruption
{
    public static class JoinAntiCorruptionResultFilter
    {
        public static IQueryable<JoinAntiCorruptionResult> ByDocNumber(this IQueryable<JoinAntiCorruptionResult> query, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return query.Where(x => x.DocNumber == docNumber);
        }
    }
}
