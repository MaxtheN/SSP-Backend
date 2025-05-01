using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.DataLayer
{
    public static class UserFilter
    {
        public static IQueryable<User> ByUserName(this IQueryable<User> source, string userName, bool isIncludePassive = false)
        {
            if (userName.NullOrEmpty())
                throw new ArgumentException($"{nameof(userName)} cannot be null or empty string", nameof(userName));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.UserName.ToLower() == userName.ToLower());
        }

        public static IQueryable<User> ByEmail(this IQueryable<User> source, string email, bool isIncludePassive = true)
        {
            if (email.NullOrEmpty())
                throw new ArgumentException($"{nameof(email)} cannot be null or empty string", nameof(email));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Email.ToLower() == email.ToLower());
        }

        //public static IQueryable<User> ByInn(this IQueryable<User> source, string inn, bool isIncludePassive = false)
        //{
        //    if (inn.NullOrEmpty())
        //        throw new ArgumentException($"{nameof(inn)} cannot be null or empty string", nameof(inn));

        //    if (!isIncludePassive)
        //        source = source.IsActive();

        //    return source.Where(a => a.Inn.ToLower() == inn.ToLower());
        //}

        public static IQueryable<User> ByPinfl(this IQueryable<User> source, string pinfl, bool isIncludePassive = false)
        {
            if (pinfl.NullOrEmpty())
                throw new ArgumentException($"{nameof(pinfl)} cannot be null or empty string", nameof(pinfl));

            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.Person.Pinfl.ToLower() == pinfl.ToLower());
        }
    }
}
