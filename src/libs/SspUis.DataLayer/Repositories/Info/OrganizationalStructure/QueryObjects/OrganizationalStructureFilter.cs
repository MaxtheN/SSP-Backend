using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public static class OrganizationalStructureFilter
    {
        public static IQueryable<OrganizationalStructure> ByNumberCode(this IQueryable<OrganizationalStructure> source, string numberCode, bool isIncludePassive = false)
        {
            if (numberCode.NullOrEmpty())
                throw new ArgumentException($"{nameof(numberCode)} cannot be null or empty string", nameof(numberCode));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Code.ToLower() == numberCode.ToLower());
        }
    }
}
