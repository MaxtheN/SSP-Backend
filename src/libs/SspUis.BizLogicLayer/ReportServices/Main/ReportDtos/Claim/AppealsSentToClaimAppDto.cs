using Newtonsoft.Json;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class AppealsSentToClaimAppDto : BaseClaimApplicationReportDto
    {
        public (long PhysicalPersonCount, long LegalPersonCount) ApplicationCount { get; set; }
        public (long PhysicalPersonCount, long LegalPersonCount) MediationPlanCount { get; set; }
        public (long PhysicalPersonCount, long LegalPersonCount) MediationCount { get; set; }
        public (long PhysicalPersonCount, long LegalPersonCount) ClaimApplicationForCourtCount { get; set; }

        [JsonIgnore]
        public long CancelApplicationCount { get; set; }
        [JsonIgnore]
        public long CancelMediationPlanCount { get; set; }
        [JsonIgnore]
        public long CancelMediationCount { get; set; }

        [JsonIgnore]
        public long RejectApplicationCount { get; set; }
        [JsonIgnore]
        public long RejectMediationPlanCount { get; set; }
        [JsonIgnore]
        public long RejectMediationCount { get; set; }

        public (long RejectedCount, long CanceledCount) RejectCancelApplicationCancel { get; set; }
    }
}