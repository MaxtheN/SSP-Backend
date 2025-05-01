using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public static class ArbitrationCourtApplicationListDtoSortFilter
    {
        public static IQueryable<ArbitrationCourtApplicationListDto> SortFilter(
            this IQueryable<ArbitrationCourtApplicationListDto> query, 
            ArbitrationCourtApplicationSortFilterOptions options)
        {
            //query = query.DocumentFilter(options);

            //if (options.StatusId.HasValue && options.StatusId != 0)
            //    query = query.Where(x => x.Application.StatusId == options.StatusId);

            if (options.ContractorResponsibleTypeId.HasValue && options.ContractorResponsibleTypeId != 0)
                query = query.Where(a => a.ContractorResponsibleTypeId == options.ContractorResponsibleTypeId.Value);

            if (options.CurrencyId.HasValue && options.CurrencyId != 0)
                query = query.Where(a => a.CurrencyId == options.CurrencyId.Value);

            if (options.ClaimResponsibleTypeId.HasValue && options.ClaimResponsibleTypeId != 0)
                query = query.Where(a => a.ClaimResponsibleTypeId == options.ClaimResponsibleTypeId.Value);

            if (options.FromDocDate.HasValue)
                query = query.Where(a => a.DocOn >= options.FromDocDate);

            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.DocOn <= options.ToDocDate);

            //if (!string.IsNullOrEmpty(options.ContractorInn))
            //    query = query.Where(a => a.Application.ContractorInn == options.ContractorInn);

            if (options.HasSearch())
                query = query.Where(a => a.Application.DocNumber.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.Contractor.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.ContractorInn.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.ContractorPhoneNumber.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
