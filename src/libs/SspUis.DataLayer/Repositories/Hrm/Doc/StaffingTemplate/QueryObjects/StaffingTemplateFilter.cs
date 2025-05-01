using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public static class StaffingTemplateFilter
    {
        public static IQueryable<StaffingTemplate> ByDocNumber(this IQueryable<StaffingTemplate> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));

            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
