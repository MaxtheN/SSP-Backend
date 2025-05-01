using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class MemshipReportDto
    {
        public string Region { get; set; }
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public bool ByRegion { get; set; } = false;

        public string District { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictOrderCode { get; set; }
        public bool ByDistrict { get; set; } = false;

        public int MemshipGeneralPlan { get; set; }
        public int MemshipFactByMonth { get; set; }
        public int MemshipFactByMonthPercentage { get; set; }

        public int MemshipApplicationLegal { get; set; }
        public int MemshipApplicationYtt { get; set; }
        public int MemshipCertificateLegal { get; set; }
        public int MemshipCertificateYtt { get; set; }

        public int MemshipGeneralPlanByYear { get; set; }
        public int MemshipFactByYear { get; set; }
        public int MemshipFactByYearPercentage { get; set; }

        public int MemshipApplicationCountByYear { get; set; }
        public int MemshipContractCountByYear { get; set; }

        public int MemshipCertificateAcceptedCountByear { get; set; }
        public int MemshipCertificateProgressCountByear { get; set; }
        public long MemshipCertificateNotIncludedCountByear { get; set; }

        public int MemshipGeneralIndebtednessByYear { get; set; }
        public int MemshipGeneralIndebtednessPercentageByYear { get; set; }
        public float MemshipGeneralIndebtednessCoefficientByYear { get; set; }

    }
}
