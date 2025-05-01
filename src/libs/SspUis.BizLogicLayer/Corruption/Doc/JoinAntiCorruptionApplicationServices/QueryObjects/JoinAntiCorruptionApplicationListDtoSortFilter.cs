using SspUis.Core;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public static class JoinAntiCorruptionApplicationListDtoSortFilter
    {
        public static IQueryable<JoinAntiCorruptionApplicationListDto> SortFilter(this IQueryable<JoinAntiCorruptionApplicationListDto> query, JoinAntiCorruptionApplicationSortFilterOptions options)
        {
            if (options.FromDocDate.HasValue)
                query = query.Where(a => options.FromDocDate.Value <= a.Application.DocOn);

            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.Application.DocOn <= options.ToDocDate.Value);

            if (options.StatusIds != null && options.StatusIds.Any())
                query = query.Where(a => options.StatusIds.Contains(a.Application.StatusId));

            if (options.WithoutCertificate)
                query = query.Where(a => a.CertificateId == null || a.CertificateStatusId != StatusIdConst.CANCELED);

            if (options.RegionId.HasValue)
                query = query.Where(a => a.Application.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.Application.DistrictId == options.DistrictId.Value);

            if (options.CurrentStepId.HasValue)
                query = query.Where(a => a.CurrentStepId == options.CurrentStepId);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => options.ContractorInn == a.Application.ContractorInn);

            if (options.HasSearch())
                query = query.Where(a => a.Application.DocNumber.ToLower().Contains(options.Search.ToLower())
                                      || a.Application.Contractor.ToLower().Contains(options.Search.ToLower())
                                      || a.Application.ContractorInn.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }

    public class JoinAntiCorruptionApplicationSortFilterOptions : DocumentSortFilterOptions
    {
        public bool WithoutCertificate { get; set; } = false;
        public string ContractorInn { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? CurrentStepId { get; set; }
        public bool ForResult { get; set; }
    }
}
