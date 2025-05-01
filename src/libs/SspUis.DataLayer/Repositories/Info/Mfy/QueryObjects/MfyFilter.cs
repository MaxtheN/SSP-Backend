using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public static class MfyFilter
    {
        public static IQueryable<Mfy> ByExternalId(this IQueryable<Mfy> source,
                                                   long externalId,
                                                   bool isIncludePassive = false)
        {
            if (!isIncludePassive)
                source = source.IsActive();

            return source.Where(a => a.ExternalId == externalId);
        }
    }
}
