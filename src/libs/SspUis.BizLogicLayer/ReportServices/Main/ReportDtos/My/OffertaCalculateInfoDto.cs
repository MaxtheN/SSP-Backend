using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class OffertaCalculateInfoDto
    {
        public int? RegionId { get; set; }
        public string Region { get; set; }
        public string RegionOrderCode { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public int Yes { get; set; }
        public int No { get; set; }

        public double TotalDocCount { get; set; }
    }
}
