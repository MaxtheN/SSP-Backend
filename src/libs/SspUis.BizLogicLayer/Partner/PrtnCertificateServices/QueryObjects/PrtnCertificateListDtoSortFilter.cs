using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public static class PrtnCertificateListDtoSortFilter
    {
        public static IQueryable<PrtnCertificateListDto> SortFilter(this IQueryable<PrtnCertificateListDto> query, PrtnDocumentSortFilterOptions options)
        {
            query = query.DocumentFilter(options);

            if (options.RegionId.HasValue)
                query = query.Where(a => a.ContractorRegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.ContractorDistrictId == options.DistrictId.Value);

            if (options.MfyId.HasValue)
                query = query.Where(a => a.MfyId == options.MfyId.Value);

            if (options.PrtnContractTypeId.HasValue)
                query = query.Where(a => a.PrtnContractTypeId == options.PrtnContractTypeId.Value);
            
            if (!options.IsTotalSuccessPost)
                query = query.Where(a => !a.IsTotalSuccessPost);

            if (options.StatusId.HasValue)
                query = query.Where(a => a.StatusId == options.StatusId.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => options.ContractorInn == a.ContractorInn);

            if (!string.IsNullOrEmpty(options.OkedCode))
                query = query.Where(a => a.OkedCode == options.OkedCode);

            if (options.HasSearch())
                query = query.Where(a => a.PrtnContractDocNumber.ToLower().Contains(options.Search.ToLower())
                                      || a.PrtnContractType.ToLower().Contains(options.Search.ToLower())
                                      || a.Contractor.ToLower().Contains(options.Search.ToLower())
                                      || a.ContractorInn.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
