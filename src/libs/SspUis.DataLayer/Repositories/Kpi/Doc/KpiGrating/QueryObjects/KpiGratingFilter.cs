using SspUis.DataLayer.EfClasses.Kpi;
using System.Linq;
using System;

namespace SspUis.DataLayer.Repositories;

public static class KpiGratingFilter
{
    public static IQueryable<KpiGrating> ByDocNumber(this IQueryable<KpiGrating> source, string docNumber)
    {
        if (string.IsNullOrEmpty(docNumber))
            throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
        return source.Where(a => a.DocNumber == docNumber);
    }
}
