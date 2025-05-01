using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public static class TimesheetFilter
    {
        public static IQueryable<Timesheet> ByDocNumber(this IQueryable<Timesheet> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));

            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
