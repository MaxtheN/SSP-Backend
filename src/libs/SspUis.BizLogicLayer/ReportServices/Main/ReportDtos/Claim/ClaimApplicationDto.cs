using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ClaimApplicationDto
    {
        public int? RegionId { get; set; }
        public int? OrganisationId { get; set; }
        public string Organisation { get; set; }
        public string Region { get; set; }
        public string RegionOrderCode { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }
        public string DistrictOrderCode { get; set; }

        public long? ContractorId { get; set; }
        public string Contractor { get; set; }

        public long? TotalClaimApplicationCount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalClaimApplicationAmount { get; set; }
        public long? TotalEconomicCourt { get; set; }
        public long? TotalCivilCourt { get; set; }
        public long? TotalAdministrativeCourt { get; set; }
        public long? TotalAppilationCount { get; set; }
        public decimal? TotalAppilationAmount { get; set; }
        public long? TotalAppilationAcceptedCount { get; set; }
        public long? TotalAppilationCanceledCount { get; set; }
        public long? TotalRevisionCount { get; set; }
        public decimal? TotalRevisionAmount { get; set; }
        public long? TotalRevisionAcceptedCount { get; set; }
        public long? TotalRevisionCanceledCount { get; set; }
        public long? TotalMediationCount { get; set; }
    }
}