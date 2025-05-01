using System;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class MemshipContractPaidDtoFilter
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public int? RegionalOrganizationId { get; set; }
        public bool ByOrganization { get; set; } = false;

        public long? ContractorId { get; set; }
        public bool ByContractor { get; set; } = false;


        public bool IsOld { get; set; } = false;
    }
}
