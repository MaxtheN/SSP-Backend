using WEBASE.Models;
using System.Linq.Dynamic.Core;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public static class StaffingListDtoSortFilter
    {
        public static IQueryable<StaffingListDto> SortFilter(this IQueryable<StaffingListDto> query, StaffingSortFilterPageOptions options)
        {
            if (options.StatusIds != null && options.StatusIds.Count() > 0)
                query = query.Where(a => options.StatusIds.Contains(a.StatusId));

            if (options.FinanceYear != null)
                query = query.Where(a => a.FinanceYear == options.FinanceYear);

            if (options.OrganizationId != null)
                query = query.Where(a => a.OrganizationId == options.OrganizationId);

            if (options.HasSearch())
                query = query.Where(a => a.DocNumber.Contains(options.Search.ToLower()) ||
                                         (!string.IsNullOrEmpty(a.SettlementAccountSource) && 
                                         a.SettlementAccountSource.Contains(options.Search.ToLower())) ||
                                          (!string.IsNullOrEmpty(a.OrganizationalClassification) &&
                                          a.OrganizationalClassification.Contains(options.Search.ToLower())) ||
                                         a.Status.Contains(options.Search.ToLower()));
            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);



            return query;
        }
    }
}
