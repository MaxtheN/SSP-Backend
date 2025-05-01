using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public static class MassPlannedCalculationFilter
    {
        public static IQueryable<MassPlannedCalculation> ByDocNumber(this IQueryable<MassPlannedCalculation> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
