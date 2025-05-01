using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public static class BusinessmanUserFilter
    {
        public static IQueryable<BusinessmanUser> ByUserId(this IQueryable<BusinessmanUser> source, int id, bool isIncludePassive = false)
        {
            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Id == id && a.StateId != StateIdConst.PASSIVE);

        }
        public static IQueryable<BusinessmanUser> ByInn(this IQueryable<BusinessmanUser> source, string inn, bool isIncludePassive = false)
        {
            if (inn.NullOrEmpty())
                throw new ArgumentException($"{nameof(inn)} cannot be null or empty string", nameof(inn));

            if (!isIncludePassive)
                source = source.IsActive();

                return source.Where(a => a.Inn.ToLower() == inn.ToLower());
        }
    }
}
