
using System.Linq;
using System;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories;

public static class MemshipNewContractorFilter
{
    public static IQueryable<MemshipNewContractor> ByDocNumber(this IQueryable<MemshipNewContractor> source, string docNumber)
    {
        if (string.IsNullOrEmpty(docNumber))
            throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
        return source.Where(a => a.DocNumber == docNumber);
    }
}