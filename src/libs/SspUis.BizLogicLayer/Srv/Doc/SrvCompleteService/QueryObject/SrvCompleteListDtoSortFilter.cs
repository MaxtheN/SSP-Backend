using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer
{
    public static class SrvCompleteListDtoSortFilter 
    {
        public static IQueryable<CompletedServiceListDto> SortFilter(this IQueryable<CompletedServiceListDto> query, SrvCompleteSortFilterOption options)
        {
            if (options.StatusId.HasValue)
                query = query.Where(c => c.StatusId == options.StatusId);

            if (options.ContractorId.HasValue)
                query = query.Where(c => c.ContractorId == options.ContractorId);

            if (options.ServiceContractId.HasValue)
                query = query.Where(c => c.ServiceContractId == options.ServiceContractId);

            if (options.ServiceContractTableId.HasValue)
                query = query.Where(c => c.ServiceContractTableId == options.ServiceContractTableId);

            if (options.EmployeeManageId.HasValue)
                query = query.Where(c => c.EmployeeManageId == options.EmployeeManageId);

            if (options.HasSearch())
                query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower())
                                      || a.Details.ToLower().Contains(options.Search.ToLower())
                                      || a.OrganizationName != null 
                                            ? a.OrganizationName.ToLower().Contains(options.Search.ToLower())
                                            : false
                                      || a.ContractorInn != null 
                                            ? a.ContractorInn.ToLower().Contains(options.Search.ToLower())
                                            : false
                                      || a.ContractorFullName != null 
                                            ? a.ContractorFullName.ToLower().Contains(options.Search.ToLower())
                                            : false
                                      || a.ContractorRegion != null 
                                            ? a.ContractorRegion.ToLower().Contains(options.Search.ToLower())
                                            : false
                                      || a.ContractorDistrict != null 
                                            ? a.ContractorDistrict.ToLower().Contains(options.Search.ToLower())
                                            : false
                                      );

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
