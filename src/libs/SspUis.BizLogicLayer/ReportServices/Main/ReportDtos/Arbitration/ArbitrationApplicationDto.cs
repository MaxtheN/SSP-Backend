using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class ArbitrationApplicationDto
    {
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public string RegionOrderCode { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }
        public string DistrictOrderCode { get; set; }

        public long? ContractorId { get; set; }
        public string Contractor { get; set; }

        public long? TotalArbitrationApplicationCount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalArbitrationApplicationAmount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalArbitrationPaidApplicationAmount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalArbitrationLatePaidApplicationAmount { get; set; }
        public long? TotalArbitrationAcceptedCount { get; set; }
        public long? TotalArbitrationPartiallyAcceptedCount { get; set; }
        public long? TotalArbitrationCanceledCount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalArbitrationAcceptedAmount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalArbitrationPartiallyAcceptedAmount { get; set; }
        public (decimal? Uzs, decimal? Usd, decimal? Euro) TotalArbitrationCanceledAmount { get; set; }
    }
}