using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices
{
    public static class PrtnCreditDemandListDtoSortFilter
    {
        public static IQueryable<PrtnCreditDemandListDto> SortFilter(this IQueryable<PrtnCreditDemandListDto> query, PrtnCreditDemandSortFilterOptions options)
        {
            if (!options.ContractorInn.IsNullOrEmpty())
                query = query.Where(d => d.ContractorInn == options.ContractorInn);

            if (options.CertificateId != null && options.CertificateId != 0)
                query = query.Where(d => d.CertificateId == options.CertificateId);

            if (options.ApplicationId != null && options.ApplicationId != 0)
                query = query.Where(d => d.ApplicationId == options.ApplicationId);

            if (options.RegionId != null && options.RegionId != 0)
                query = query.Where(d => d.RegionId == options.RegionId);

            if (options.StatusId != null && options.StatusId != 0)
                query = query.Where(d => d.StatusId == options.StatusId);

            if (options.DistrictId != null && options.DistrictId != 0)
                query = query.Where(d => d.DistrictId == options.DistrictId);

            if (options.PrtnContractTypeId != null && options.PrtnContractTypeId != 0)
                query = query.Where(d => d.PrtnContractTypeId == options.PrtnContractTypeId);

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
