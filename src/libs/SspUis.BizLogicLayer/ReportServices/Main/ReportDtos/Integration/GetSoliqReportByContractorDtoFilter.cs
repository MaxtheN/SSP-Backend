using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class GetSoliqReportByContractorDtoFilter//: PageOptions
    {
        public bool ByRegion { get; set; }
        
        public bool ByDistrict { get; set; }    
        public bool ByContractor { get; set; }
        public bool HasCertificate { get; set; }    
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public long? Id { get; set; }
        public string Inn { get; set; }
        public int? Year { get; set; }
    }
}
