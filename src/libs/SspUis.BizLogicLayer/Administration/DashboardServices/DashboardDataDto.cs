using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.DashboardServices
{
    public class DashboardDataDto
    {
        public DashboardAgreementResultDto AgreementResult { get; set; }
        public DashboardInspectionStatusDto InspectionStatus { get; set; }
        public List<DashboardDataByRegionDto> ByRegions { get; set; }
    }

    public class DashboardDataByRegionDto
    {
        public int RegionId { get; set; }
        public string Region { get; set; }
        public long InspectionsCount { get; set; }
        public long BeginnedCount { get; set; }
        public DateTime? LastInspectionStartDate { get; set; }
    }

    public class DashboardAgreementResultDto
    {
        public long AgreedCount { get; set; }
        public long RejectedCount { get; set; }
        public long AnalysingCount { get; set; }
    }

    public class DashboardInspectionStatusDto
    {
        public long NewsCount { get; set; }
        public long ExpiredCount { get; set; }
        public long ProlongedCount { get; set; }
        public long PostponedCount { get; set; }
        public long CancelledCount { get; set; }
    }
}
