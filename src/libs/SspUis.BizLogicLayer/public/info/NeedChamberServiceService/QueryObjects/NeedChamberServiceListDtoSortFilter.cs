using SspUis.DataLayer;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices
{
    public static class NeedChamberServiceListDtoSortFilter
    {
        public static IQueryable<NeedChamberServiceListDto> SortFilter(this IQueryable<NeedChamberServiceListDto> query, NeedChamberServiceSortFilterDto options)
        {
            if (options.ServicePriceTypeId.HasValue)
                query = query.Where(x => x.ServicePriceTypeId == options.ServicePriceTypeId);

            if (options.IsPaid.HasValue)
            {
                query = options.IsPaid.Value
                    ? query.Where(x => x.ServicePriceTypeId == ServicePriceTypeIdConst.BY_AGREEMENT
                        || x.ServicePriceTypeId == ServicePriceTypeIdConst.BXM
                        || x.ServicePriceTypeId == ServicePriceTypeIdConst.PERCENTAGE_CONTRACT_SIZE)
                    : query.Where(x => x.ServicePriceTypeId == ServicePriceTypeIdConst.FREE);
            }

            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Code.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
