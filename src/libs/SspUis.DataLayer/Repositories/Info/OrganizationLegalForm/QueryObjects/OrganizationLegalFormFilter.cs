using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public static class OrganizationLegalFormFilter
    {
        public static IQueryable<OrganizationLegalForm> ByNumberCode(this IQueryable<OrganizationLegalForm> source, string numberCode, bool isIncludePassive = false)
        {
            if (numberCode.NullOrEmpty())
                throw new ArgumentException($"{nameof(numberCode)} cannot be null or empty string", nameof(numberCode));

            return source;
        }
    }
}
