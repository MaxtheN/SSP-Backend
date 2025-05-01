using SspUis.DataLayer.EfClasses.Memship;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Memship
{
    public static class MemshipYearlyPlanFilter
    {
        public static IQueryable<MemshipYearlyPlan> ByDocNumber(this IQueryable<MemshipYearlyPlan> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
