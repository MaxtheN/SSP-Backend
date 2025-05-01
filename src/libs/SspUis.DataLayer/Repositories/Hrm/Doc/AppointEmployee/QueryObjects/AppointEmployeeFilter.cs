using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public static class AppointEmployeeFilter
    {
        public static IQueryable<AppointEmployee> ByDocNumber(this IQueryable<AppointEmployee> query, string docNumber, int currentYear)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return query.Where(x => x.DocNumber == docNumber && x.DocOn.Year == currentYear);
        }
    }
}
