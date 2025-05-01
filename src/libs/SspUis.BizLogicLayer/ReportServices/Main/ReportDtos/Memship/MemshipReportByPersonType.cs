using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Memship
{
    public class MemshipReportByPersonType
    {
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictOrderCode { get; set; }
        public string District { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public TotalAcceptedApplication TotalAcceptedApplication { get; set; }
        public TotalSignedApplication TotalSignedApplication { get; set; }
        public TotalGivenCertificateCount TotalGivenCertificateCount { get; set; }
    }
    public class TotalAcceptedApplication
    {
        public int LegalPersonCount { get; set; }
        public int PhysicalPersonCount { get; set; }
    }
    public class TotalSignedApplication
    {
        public int LegalPersonCount { get; set; }
        public int PhysicalPersonCount { get; set; }
    }
    public class TotalGivenCertificateCount
    {
        public int LegalPersonCount { get; set; }
        public int PhysicalPersonCount { get; set; }
    }
}