using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Claim
{
    public static class MediationPlanFilter
    {
        public static IQueryable<MediationPlan> ByDocNumber(this IQueryable<MediationPlan> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
