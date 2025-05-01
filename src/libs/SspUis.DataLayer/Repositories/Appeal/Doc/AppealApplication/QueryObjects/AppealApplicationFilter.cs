using SspUis.DataLayer.EfClasses.Appeal;
using System;
using System.Linq;

namespace SspUis.DataLayer.Repositories.Appeal
{
    public static class AppealApplicationFilter
    {
        public static IQueryable<AppealApplication> ByDocNumber(this IQueryable<AppealApplication> source, string docNumber)
        {
            if (string.IsNullOrEmpty(docNumber))
                throw new ArgumentException($"{nameof(docNumber)} cannot be null or empty string", nameof(docNumber));
            return source.Where(a => a.DocNumber == docNumber);
        }
    }
}
