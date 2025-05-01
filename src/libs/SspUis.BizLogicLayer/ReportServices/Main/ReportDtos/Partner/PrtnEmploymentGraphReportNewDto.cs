using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Partner
{
    public class PrtnEmploymentGraphReportNewDto
    {
        public int? RegionId { get; set; }
        public string? RegionOrderCode { get; set; }
        public string? Region { get; set; }
        public int? DistrictId { get; set; }
        public string? District { get; set; }
        public long? MfyId { get; set; }
        public string? Mfy { get; set; }
        public long? ContractorId { get; set; }
        public string? ContractorInn { get; set; }
        public string? OrganizationName { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? SentForExamination {  get; set; }
        public DateTime? DateOfConclusionByJustice { get; set; }
        public int? DaysLate { get; set; }
        public string? Status { get; set; }
        public int? StatusId { get; set; }
    }
}
