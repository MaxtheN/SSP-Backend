using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.ServiceLayer.NumberServices
{
    public static class NumberFilter
    {
        public static NumberTemplate GetActual(this IQueryable<NumberTemplate> query, string document) =>
            query
                .Where(a => a.Document == document)
                .OrderByDescending(a => a.Date)
                .FirstOrDefault();

        public static Number GetCurrent(this IQueryable<Number> query, string document, int organizationId) =>
            query.FirstOrDefault(a => a.OrganizationId == organizationId && a.Document == document);


    }
}
