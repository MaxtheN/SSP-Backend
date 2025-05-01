using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class BankCreditApplicationReportByRegionAndDistrictDtoFilter
    {
        public int? RegionId { get; set; }
        public bool ByRegion { get; set; } = false;

        public int? DistrictId { get; set; }
        public bool ByDistrict { get; set; } = false;

        public long? ContractorId { get; set; }
        public string? ContractorInn { get; set; }
        public bool ByContractor { get; set; } = false;
        public int? LanguageId { get; set; }
        public int? ContractTypeId { get; set; }
        public bool ByContactType { get; set; }
        public bool HasCertificate { get; set; } = true;
    }
}
