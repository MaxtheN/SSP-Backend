using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class RestrictionSendingAppSortFilter
    {
        public static IQueryable<RestrictionSendingAppListDto> SortFilter(
            this IQueryable<RestrictionSendingAppListDto> query,
            RestrictionSendingAppDtoSortFilter options)
        {
            if (options.TableId.HasValue)
                query = query.Where(a => a.TableId == options.TableId.Value);

            if (options.HasSearch())
                query = query.Where(a => a.MessageText.ToLower().Contains(options.Search.ToLower())
                                      || a.Details.ToLower().Contains(options.Search.ToLower()));
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
