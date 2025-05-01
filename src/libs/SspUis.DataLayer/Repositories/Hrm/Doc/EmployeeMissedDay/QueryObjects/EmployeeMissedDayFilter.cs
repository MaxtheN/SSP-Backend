using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;

namespace SspUis.DataLayer
{
    public static class EmployeeMissedDayFilter
    {
        public static IQueryable<EmployeeMissedDay> ByDocNumber(this IQueryable<EmployeeMissedDay> source, string docNumber)
        {
            if (docNumber.NullOrEmpty())
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));

            return source.Where(a => a.DocNumber.ToLower() == docNumber.ToLower());
        }
    }
}
