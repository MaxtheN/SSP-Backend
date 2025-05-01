

using SspUis.DataLayer.EfClasses.Kpi;
using System.Linq;
using System;

namespace SspUis.DataLayer.Repositories.Kpi;



public static class KpiPlanForEmployeeFilter
{
    public static IQueryable<KpiPlanForEmployee> ByDocNumber(this IQueryable<KpiPlanForEmployee> source, string docNumber)
    {
        if (string.IsNullOrEmpty(docNumber))
            throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
        return source.Where(a => a.DocNumber == docNumber);
    }
}