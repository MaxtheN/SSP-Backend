using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main.QueryObjects
{
    public class ExpiredContractorsReportDto
    {
        public string Region { get; set; }
        public int RegionId { get; set; }
        public string ContractorInn {  get; set; }
        public string District { get; set; }
        public int DistrictId { get; set; }

        public string Contractor { get; set; }
        public int ContractorId { get; set; }

        public int AllApplicationsCount { get; set; }
        public long IsBeingCosideredApplicationCount { get; set; }
        public int ExpiredIsBeingCosideredApplicationCount { get; set; }
        public int SendToExpertiseCount { get; set; }
        public int ExpiredSendToExpertiseCount { get; set; }
        public int NotPassCount { get; set; }
        public int ExpiredResentToExpiredCount { get; set; }

        public int PassCount1 { get; set; }
        public int SignExpireOnCount1 { get; set; }

        public int PassCount2 { get; set; }
        public int SigningExpireOnCount { get; set; }

        public int SigningCount { get; set; }
        public int SignExpireOnCount2 { get; set; }

        public int NotGeneratedCertificatesCount { get; set; }

        public int GeneratedCertificatesCount { get; set; }
    }
}
