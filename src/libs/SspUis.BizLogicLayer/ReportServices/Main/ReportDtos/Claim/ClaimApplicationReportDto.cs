using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ClaimApplicationReportDto
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

        #region sud turlari
        public long? TotalEconomicCourt { get; set; }
        public long? TotalCivilCourt { get; set; }
        public long? TotalAdministrativeCourt { get; set; }
        #endregion

        public long? LeganClaims { get; set; }
        public long? SatisfiedClaims { get; set; }
        public long? CanceledClaims { get; set; }
        public long? RejectedClaims { get; set; }

        public long? TotalAppilationCount { get; set; }
        public decimal? TotalAppilationAmount { get; set; }
        public long? TotalAppilationAcceptedCount { get; set; }
        public long? TotalAppilationRejectedCount { get; set; }
        public long? TotalMediationCount { get; set; }
    }


}
